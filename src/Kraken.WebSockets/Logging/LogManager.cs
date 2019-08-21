using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kraken.WebSockets.Logging
{
    internal static class LogManager
    {
        /// <summary>
        /// Gets or sets the logger factory.
        /// </summary>
        /// <value>
        /// The logger factory.
        /// </value>
        internal static ILoggerFactory LoggerFactory { get; set; } = new NullLoggerFactory();

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ILogger<T> GetLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }
    }
}
