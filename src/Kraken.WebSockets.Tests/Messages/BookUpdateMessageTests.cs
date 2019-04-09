using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class BookUpdateMessageTests
    {
        #region CreateFromStrimg()

        [Fact]
        public void CreateFromString_TestComplete()
        {
            var rawBookupdateMessage = @"[
  1234,
  {""a"": [
    [
      ""5541.30000"",
      ""2.50700000"",
      ""1534614248.456738""
    ],
    [
      ""5542.50000"",
      ""0.40100000"",
      ""1534614248.456738""
    ]
  ]
  },
  {""b"": [
    [
      ""5541.30000"",
      ""0.00000000"",
      ""1534614335.345903""
    ]
  ]
  }
]";
            var bookUpdateMessage = BookUpdateMessage.CreateFromString(rawBookupdateMessage);

            Assert.NotNull(bookUpdateMessage);
            Assert.Equal(1234, bookUpdateMessage.ChannelId);
            Assert.NotNull(bookUpdateMessage.Asks);
            Assert.Equal(2, bookUpdateMessage.Asks.Length);
            Assert.NotNull(bookUpdateMessage.Bids);
            Assert.Single(bookUpdateMessage.Bids);
        }

        [Fact]
        public void CreateFromString_TestOnlyAsks()
        {
            var rawBookupdateMessage = @"[1234,{""a"": [[""5541.30000"",""2.50700000"",""1534614248.456738""],[""5542.50000"",""0.40100000"",""1534614248.456738""]]}]";
            var bookUpdateMessage = BookUpdateMessage.CreateFromString(rawBookupdateMessage);

            Assert.NotNull(bookUpdateMessage);
            Assert.Equal(1234, bookUpdateMessage.ChannelId);
            Assert.NotNull(bookUpdateMessage.Asks);
            Assert.Equal(2, bookUpdateMessage.Asks.Length);
            Assert.Null(bookUpdateMessage.Bids);
        }

        [Fact]
        public void CreateFromString_TestOnlyBids()
        {
            var rawBookupdateMessage = @"[1234,{""b"": [[""5541.30000"",""0.00000000"",""1534614335.345903""]]}]";
            var bookUpdateMessage = BookUpdateMessage.CreateFromString(rawBookupdateMessage);

            Assert.NotNull(bookUpdateMessage);
            Assert.Equal(1234, bookUpdateMessage.ChannelId);
            Assert.Null(bookUpdateMessage.Asks);
            Assert.NotNull(bookUpdateMessage.Bids);
            Assert.Single(bookUpdateMessage.Bids);
        }

        #endregion
    }
}
