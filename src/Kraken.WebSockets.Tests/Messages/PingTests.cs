using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class PingTests
    {
        #region Ctor

        [Fact]
        public void Ctor_Default_InstanceIsCreated()
        {
            Assert.NotNull(new Ping());
        }

        #endregion

        #region Event

        [Fact]
        public void Event_ReturnsPing()
        {
            Assert.Equal("ping", new Ping().Event);
        }

        #endregion

        #region RequestId

        [Fact]
        public void RequestId_ShouldReturnNullOnDefaultCtor()
        {
            Assert.Null(new Ping().RequestId);
        }

        #endregion
    }
}
