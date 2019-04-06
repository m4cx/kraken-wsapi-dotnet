using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class TradesMessageTests
    {
        #region CreateFromString() 

        [Fact]
        public void CreateFromString_()
        {
            var rawMessageString = @"[0,[[""5541.20000"",""0.15850568"",""1534614057.321597"",""s"",""l"",""""],[""6060.00000"",""0.02455000"",""1534614057.324998"",""b"",""l"",""""]]]";
            var tradesMessage = TradeMessage.CreateFromString(rawMessageString);

            Assert.NotNull(tradesMessage);
            Assert.Equal(0, tradesMessage.ChannelId);
            Assert.Equal(2, tradesMessage.Trades.Length);
        }

        #endregion
    }
}
