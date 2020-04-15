using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class OwnTradesMessageTests
    {
        [Fact]
        public void CreateFromString()
        {
            var ownTrades = OwnTradesMessage.CreateFromString(TestSocketMessages.OwnTradesMessage);

            Assert.Equal(SubscribeOptionNames.OwnTrades, ownTrades.Name);
            Assert.Equal(4, ownTrades.Trades.Count);
            Assert.True(ownTrades.Trades.All(x => x != null));
        }
    }
}
