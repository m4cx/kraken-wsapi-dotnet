using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class SpreadMessageTests
    {
        #region CreateFromString()

        [Fact]
        public void CreateFromString_Test()
        {
            var rawSpreadMessage = TestSocketMessages.SpreadMessage;
            var spreadMessage = SpreadMessage.CreateFromString(rawSpreadMessage);

            Assert.NotNull(spreadMessage);
            Assert.Equal(0, spreadMessage.ChannelId);
            Assert.Equal(5698.40000M, spreadMessage.Bid);
            Assert.Equal(5700.00000M, spreadMessage.Ask);
            Assert.Equal(1542057299.545897M, spreadMessage.Time);
        }

        #endregion
    }
}
