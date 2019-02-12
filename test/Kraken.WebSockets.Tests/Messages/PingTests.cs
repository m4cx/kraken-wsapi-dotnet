using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        public void RequestId_ShouldReturnValueFromCtor(int requestId)
        {
            Assert.Equal(requestId, new Ping(requestId).RequestId);
        }

        #endregion
    }
}
