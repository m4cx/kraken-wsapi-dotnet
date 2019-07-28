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

        #endregion

        #region GetLogger()

        [Fact]
        public void GetLogger_ReturnsLoggerAsDefault()
        {
            Assert.IsAssignableFrom<ILogger<object>>(LogManager.GetLogger<object>());
        }

        #endregion
    }
}
