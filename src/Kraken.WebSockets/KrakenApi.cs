using Kraken.WebSockets.Messages;
using System;

namespace Kraken.WebSockets
{
    /// <summary>
    /// This class represents the central entry point.
    /// </summary>
    public static class KrakenApi
    {
        private static readonly Lazy<IKrakenApiClientFactory> factory = 
            new Lazy<IKrakenApiClientFactory>(() => new KrakenApiClientFactory(new KrakenMessageSerializer()));

        /// <summary>
        /// Gets the client factory.
        /// </summary>
        /// <value>
        /// The client factory.
        /// </value>
        public static IKrakenApiClientFactory ClientFactory
        {
            get
            {
                return factory.Value;
            }
        }
    }
}
