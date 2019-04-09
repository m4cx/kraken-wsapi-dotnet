using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class TradeMessageTests
    {
        #region CreateFromString() 

        [Fact]
        public void CreateFromString_()
        {
            var rawMessageString = TestSocketMessages.TradeMessage;
            var tradesMessage = TradeMessage.CreateFromString(rawMessageString);

            Assert.NotNull(tradesMessage);
            Assert.Equal(0, tradesMessage.ChannelId);
            Assert.Equal(2, tradesMessage.Trades.Length);
        }

        #endregion
    }
}
