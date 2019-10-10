using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Kraken.WebSockets.Logging
{
    internal class LogManager
    {
        private static readonly object lockObject = new object();
        private static ILoggerFactory _factory;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                lock (lockObject)
                {
                    if (_factory == null)
                    {
                        _factory = new NullLoggerFactory();
                    }

                    return _factory;
                }
            }
            set
            {
                lock (lockObject)
                {
                    _factory = value;
                }
            }
        }
        public static ILogger<T> CreateLogger<T>()
        {
            lock (lockObject)
            {
                return LoggerFactory.CreateLogger<T>();
            }
        }
    }
}
