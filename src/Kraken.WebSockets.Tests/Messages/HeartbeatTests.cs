using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class HeartbeatTests
    {
        private readonly Heartbeat instance;

        public HeartbeatTests()
        {
            instance = new Heartbeat();
        }

        #region Event

        [Fact]
        public void Event_Get_ReturnsHeartbeat()
        {
            Assert.Equal("heartbeat", instance.Event);
        }

        #endregion
    }
}
