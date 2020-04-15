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
            var rawBookupdateMessage = TestSocketMessages.BookUpdateCompleteMessage;
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
            var rawBookupdateMessage = TestSocketMessages.BookUpdateMessageOnlyAsks;
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
            var rawBookupdateMessage = TestSocketMessages.BookUpdateMessageOnlyBids;
            var bookUpdateMessage = BookUpdateMessage.CreateFromString(rawBookupdateMessage);

            Assert.NotNull(bookUpdateMessage);
            Assert.Equal(1234, bookUpdateMessage.ChannelId);
            Assert.Null(bookUpdateMessage.Asks);
            Assert.NotNull(bookUpdateMessage.Bids);
            Assert.Single(bookUpdateMessage.Bids);
        }

        [Fact]
        public void CreateFromString_TestAsksRepublished()
        {
            var rawBookupdateMessage = TestSocketMessages.BookUpdateAsksRepublished;
            var bookUpdateMessage = BookUpdateMessage.CreateFromString(rawBookupdateMessage);

            Assert.NotNull(bookUpdateMessage);
            Assert.Equal(1234, bookUpdateMessage.ChannelId);
            Assert.NotNull(bookUpdateMessage.Asks);
            Assert.Equal(2, bookUpdateMessage.Asks.Length);
            Assert.Null(bookUpdateMessage.Bids);

            #endregion
        }
    }
}
