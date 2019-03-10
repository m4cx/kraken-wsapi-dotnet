using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Extensions;
using Kraken.WebSockets.Messages;
using Serilog;

namespace Kraken.WebSockets
{
    /// <summary>
    /// Kraken API client.
    /// </summary>
    public sealed class KrakenApiClient : IKrakenApiClient
    {
        private static readonly ILogger logger = Log.ForContext<KrakenApiClient>();

        private readonly IKrakenSocket socket;
        private readonly IKrakenMessageSerializer serializer;

        /// <summary>
        /// Gets the system status.
        /// </summary>
        /// <value>The system status.</value>
        public SystemStatus SystemStatus { get; private set; }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <value>The subscriptions.</value>
        public IDictionary<int, SubscriptionStatus> Subscriptions { get; } = new Dictionary<int, SubscriptionStatus>();

        /// <summary>
        /// Occurs when system status changed.
        /// </summary>
        public event EventHandler<KrakenMessageEventArgs<SystemStatus>> SystemStatusChanged;

        /// <summary>
        /// Occurs when subscription status changed.
        /// </summary>
        public event EventHandler<KrakenMessageEventArgs<SubscriptionStatus>> SubscriptionStatusChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.KrakenApiClient" /> class.
        /// </summary>
        /// <param name="socket">Socket.</param>
        /// <param name="serializer">Serializer.</param>
        /// <exception cref="ArgumentNullException">
        /// socket
        /// or
        /// serializer
        /// </exception>
        public KrakenApiClient(IKrakenSocket socket, IKrakenMessageSerializer serializer)
        {
            logger.Debug("Creating a new client instance");
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            // Add watch for incoming messages 
            logger.Debug("Applying incoming message handler");
            this.socket.DataReceived += HandleIncomingMessage;
        }

        /// <summary>
        /// Creates a subscription.
        /// </summary>
        /// <param name="subscribe">The subscription.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">subscribe</exception>
        public async Task SubscribeAsync(Subscribe subscribe)
        {
            if (subscribe == null)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            logger.Debug("Adding subscription: {@subscribe}", subscribe);
            await socket.SendAsync(subscribe);
        }

        /// <summary>
        /// Unsubscribe from a specific subscription.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">subscription</exception>
        public async Task UnsubscribeAsync(int channelId)
        {
            if (channelId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(channelId));
            }

            logger.Debug("Unsubscribe from subscription with channelId '{channelId}'", channelId);
            await socket.SendAsync(new Unsubscribe(channelId));   
        }

        #region Private Helper

        private void HandleIncomingMessage(object sender, KrakenMessageEventArgs eventArgs)
        {
            logger.Debug("Handling incoming '{event}' message", eventArgs.Event);

            switch (eventArgs.Event)
            {
                case SystemStatus.EventName:
                    var systemStatus = serializer.Deserialize<SystemStatus>(eventArgs.RawContent);
                    logger.Verbose("System status changed: {@systemStatus}", systemStatus);
                    SystemStatus = systemStatus;
                    SystemStatusChanged.InvokeAll(this, systemStatus);
                    break;

                case SubscriptionStatus.EventName:
                    var subscriptionStatus = serializer.Deserialize<SubscriptionStatus>(eventArgs.RawContent);
                    logger.Verbose("Subscription status changed: {@subscriptionStatus}", subscriptionStatus);

                    SynchronizeSubscriptions(subscriptionStatus);
                    SubscriptionStatusChanged.InvokeAll(this, subscriptionStatus);

                    break;

                default:
                    logger.Warning("Could not handle incoming message: {@message}", eventArgs.RawContent);
                    break;
            }
        }

        private void SynchronizeSubscriptions(SubscriptionStatus currentStatus)
        {
            if (currentStatus.ChannelId == null || !currentStatus.ChannelId.HasValue)
            {
                logger.Warning("SubscriptionStatus has no channelID");
                // no channelID --> error?
                return;
            }

            // handle unsubscribe
            var channelIdValue = currentStatus.ChannelId.Value;
            if (currentStatus.Status == "unsubscribed")
            {
                if (!Subscriptions.ContainsKey(channelIdValue)) return;

                Subscriptions.Remove(channelIdValue);
                logger.Debug("Subscription for {channelID} successfully removed", channelIdValue);
                return;
            }

            // handle subscription
            var value = channelIdValue;
            if (Subscriptions.ContainsKey(value))
            {
                Subscriptions[value] = currentStatus;
            }
            else
            {
                Subscriptions.Add(value, currentStatus);
            }
        }
        
        #endregion
    }
}
