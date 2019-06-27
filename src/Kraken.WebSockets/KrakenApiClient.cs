using System;
using System.Collections.Generic;
using System.Linq;
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
    internal sealed class KrakenApiClient : IKrakenApiClient
    {
        private static readonly ILogger logger = Log.ForContext<KrakenApiClient>();

        private readonly IKrakenSocket socket;
        private readonly IKrakenMessageSerializer serializer;

        private bool disposedValue = false;

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
        /// Occurs when a new ticker information was received.
        /// </summary>
        public event EventHandler<KrakenDataEventArgs<TickerMessage>> TickerReceived;

        /// <summary>
        /// Occurs when new ohlc information was received.
        /// </summary>
        public event EventHandler<KrakenDataEventArgs<OhlcMessage>> OhlcReceived;

        /// <summary>
        /// Occurs when new trade information was received.
        /// </summary>
        public event EventHandler<KrakenDataEventArgs<TradeMessage>> TradeReceived;

        /// <summary>
        /// Occurs when new spread information was received.
        /// </summary>
        public event EventHandler<KrakenDataEventArgs<SpreadMessage>> SpreadReceived;

        /// <summary>
        /// Occurs when new book snapshot information was received.
        /// </summary>
        public event EventHandler<KrakenDataEventArgs<BookSnapshotMessage>> BookSnapshotReceived;

        /// <summary>
        /// Occurs when new book update information was received.
        /// </summary>
        public event EventHandler<KrakenDataEventArgs<BookUpdateMessage>> BookUpdateReceived;

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
        internal KrakenApiClient(IKrakenSocket socket, IKrakenMessageSerializer serializer)
        {
            logger.Debug("Creating a new client instance");
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            // Add watch for incoming messages 
            logger.Debug("Applying incoming message handler");
            this.socket.DataReceived += HandleIncomingMessage;
        }

        /// <summary>
        /// Connects to the websocket endpoint.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ConnectAsync()
        {
            logger.Debug($"Connect to the websocket");
            await socket.ConnectAsync();
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
                logger.Error("No subscribe options passed to method");
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

        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Unsubscribe from all active subscriptions
                    if (Subscriptions.Any())
                    {
                        foreach(var subscription in Subscriptions.Keys)
                        {
                            UnsubscribeAsync(subscription).GetAwaiter().GetResult();
                        }
                    }

                    socket.CloseAsync().GetAwaiter().GetResult();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

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

                case "data":

                    var subscription = Subscriptions.ContainsKey(eventArgs.ChannelId.Value) ? Subscriptions[eventArgs.ChannelId.Value] : null;
                    if (subscription == null)
                    {
                        logger.Warning($"Didn't find a subscription for channelId={eventArgs.ChannelId}");
                        break;
                    }

                    var dataType = subscription.Subscription.Name;

                    if (dataType == SubscribeOptionNames.Ticker)
                    {
                        var tickerMessage = TickerMessage.CreateFromString(eventArgs.RawContent, subscription);
                        TickerReceived.InvokeAll(this, new TickerEventArgs(subscription.ChannelId.Value, subscription.Pair, tickerMessage));
                    }
                    if (dataType == SubscribeOptionNames.OHLC)
                    {
                        var ohlcMessage = OhlcMessage.CreateFromString(eventArgs.RawContent);
                        OhlcReceived.InvokeAll(this, new OhlcEventArgs(subscription.ChannelId.Value, subscription.Pair, ohlcMessage));
                    }
                    if (dataType == SubscribeOptionNames.Trade)
                    {
                        var tradeMessage = TradeMessage.CreateFromString(eventArgs.RawContent);
                        TradeReceived.InvokeAll(this, new TradeEventArgs(subscription.ChannelId.Value, subscription.Pair, tradeMessage));
                    }
                    if (dataType == SubscribeOptionNames.Spread)
                    {
                        var spreadMessage = SpreadMessage.CreateFromString(eventArgs.RawContent);
                        SpreadReceived.InvokeAll(this, new SpreadEventArgs(subscription.ChannelId.Value, subscription.Pair, spreadMessage));
                    }
                    if (dataType == SubscribeOptionNames.Book)
                    {
                        if (eventArgs.RawContent.Contains(@"""as"":") && eventArgs.RawContent.Contains(@"""bs"":"))
                        {
                            var bookSnapshotMessage = BookSnapshotMessage.CreateFromString(eventArgs.RawContent);
                            BookSnapshotReceived.InvokeAll(this, new KrakenDataEventArgs<BookSnapshotMessage>(eventArgs.ChannelId.Value, subscription.Pair, bookSnapshotMessage));
                        }
                        if (eventArgs.RawContent.Contains(@"""a"":") || eventArgs.RawContent.Contains(@"""b"":"))
                        {
                            var bookUpdateMessage = BookUpdateMessage.CreateFromString(eventArgs.RawContent);
                            BookUpdateReceived.InvokeAll(this, new KrakenDataEventArgs<BookUpdateMessage>(eventArgs.ChannelId.Value, subscription.Pair, bookUpdateMessage));
                        }
                    }

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
