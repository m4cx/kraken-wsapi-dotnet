using System;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void InvokeAll<TMessage>(this EventHandler<KrakenMessageEventArgs<TMessage>> eventHandler, object sender, TMessage message)
            where TMessage : IKrakenMessage, new()
        {
            var instance = eventHandler;
            if (instance != null)
            {
                var handlers = instance.GetInvocationList();
                if (handlers != null)
                {
                    foreach (var handler in handlers)
                    {
                        handler.DynamicInvoke(sender, new KrakenMessageEventArgs<TMessage>(message));
                    }
                }
            }
        }
    }
}
