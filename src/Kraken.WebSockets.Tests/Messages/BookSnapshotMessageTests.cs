using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class BookSnapshotMessageTests
    {
        #region CreateFromString()

        [Fact]
        public void CreateFromString_Test()
        {
            var rawBookSnapshotMessage = TestSocketMessages.BookSnapshotMessage;
            var bookSnapshotMessage = BookSnapshotMessage.CreateFromString(rawBookSnapshotMessage);

            Assert.NotNull(bookSnapshotMessage);
            Assert.Equal(0, bookSnapshotMessage.ChannelId);
            Assert.NotNull(bookSnapshotMessage.Asks);
            Assert.Equal(3, bookSnapshotMessage.Asks.Length);
            Assert.NotNull(bookSnapshotMessage.Bids);
            Assert.Equal(3, bookSnapshotMessage.Bids.Length);
        }

        #endregion
    }
}
