using System;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets.Extensions
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invokes all registered event handlers.
        /// </summary>
        /// <param name="eventHandler">Event handler.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        /// <typeparam name="TMessage">The 1st type parameter.</typeparam>
        public static void InvokeAll<TMessage>(this EventHandler<KrakenMessageEventArgs<TMessage>> eventHandler, object sender, TMessage message)
            where TMessage : IKrakenMessage, new()
        {
            var invocationList = eventHandler?.GetInvocationList();
            if (invocationList == null) return;
            foreach (var handler in invocationList)
            {
                handler.DynamicInvoke(sender, new KrakenMessageEventArgs<TMessage>(message));
            }
        }

        public static void InvokeAll(this EventHandler<TickerEventArgs> tickerEventHandler, object sender, TickerEventArgs eventArgs)
        {
            var invocationList = tickerEventHandler?.GetInvocationList();
            if (invocationList == null) return;
            foreach (var handler in invocationList)
            {
                handler.DynamicInvoke(sender, eventArgs);
            }
        }
    }
}
