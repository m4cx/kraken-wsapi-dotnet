using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Kraken.WebSockets.Tests.Logging
{
    [ExcludeFromCodeCoverage]
    public class LogManagerTests
    {
        #region LoggerFactory

        [Fact]
        public void LoggerFactory_Get_ReturnsNullLoggerFactoryAsDefault()
        {
            Assert.IsAssignableFrom<ILoggerFactory>(LogManager.LoggerFactory);
        }

        [Fact]
        public void LoggerFactory_Get_ForceNullToReturnNullLoggerFactory()
        {
            LogManager.LoggerFactory = null;
            var factory = LogManager.LoggerFactory;

            Assert.NotNull(factory);
            Assert.IsType<NullLoggerFactory>(factory);
        }

        #endregion

        #region GetLogger()

        [Fact]
        public void GetLogger_ReturnsLoggerAsDefault()
        {
            var testLogger = LogManager.CreateLogger<LogManagerTests>();
            Assert.NotNull(testLogger);
            Assert.IsAssignableFrom<ILogger<object>>(testLogger);
        }

        #endregion
    }
}
