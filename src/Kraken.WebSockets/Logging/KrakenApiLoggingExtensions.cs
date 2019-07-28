using System;
using Microsoft.Extensions.Logging;

namespace Kraken.WebSockets.Logging
{
    /// <summary>
    /// Provides Extension methods for Logging 
    /// </summary>
    public static class KrakenApiLoggingExtensions
    {
        /// <summary>
        /// Enables the logging for the Kraken API.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <returns></returns>
        public static ILoggerFactory AddKrakenWebSockets(this ILoggerFactory loggerFactory)
        {
            LogManager.LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            return loggerFactory;
        }
    }
}
