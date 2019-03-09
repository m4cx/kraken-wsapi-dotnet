using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets
{
    public interface IKrakenApiClient
    {
        /// <summary>
        /// Gets the system status.
        /// </summary>
        /// <value>The system status.</value>
        SystemStatus SystemStatus { get; }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <value>The subscriptions.</value>
        IDictionary<int, SubscriptionStatus> Subscriptions { get; }

        /// <summary>
        /// Occurs when system status changed.
        /// </summary>
        event EventHandler<KrakenMessageEventArgs<SystemStatus>> SystemStatusChanged;

        /// <summary>
        /// Occurs when subscription status changed.
        /// </summary>
        event EventHandler<KrakenMessageEventArgs<SubscriptionStatus>> SubscriptionStatusChanged;

        Task SubscribeAsync(Subscribe subscribe);
    }
}