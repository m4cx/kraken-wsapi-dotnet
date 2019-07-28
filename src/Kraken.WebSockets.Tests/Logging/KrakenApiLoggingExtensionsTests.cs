using System;
using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kraken.WebSockets.Tests.Logging
{
    [ExcludeFromCodeCoverage]
    public class KrakenApiLoggingExtensionsTests
    {
        #region EnableLogging()

        [Fact]
        public void EnableLogging_LoggingFactoryNull_ThrowsArgumentNullException()
        {
            Assert.Equal("loggerFactory",
                Assert.Throws<ArgumentNullException>(
                    () => KrakenApiLoggingExtensions.EnableLogging(null)).ParamName);
        }

        [Fact]
        public void EnableLogging_LoggerFactory_IsSetToLogManager()
        {
            var factoryMock = new Mock<ILoggerFactory>();

            factoryMock.Object.EnableLogging();

            Assert.Equal(factoryMock.Object, LogManager.LoggerFactory);
        }

        #endregion
    }
}
