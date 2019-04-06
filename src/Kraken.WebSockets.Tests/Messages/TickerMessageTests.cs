using Kraken.WebSockets.Messages;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class TickerMessageTests
    {
        private Mock<SubscriptionStatus> subscription;

        public TickerMessageTests()
        {
            subscription = new Mock<SubscriptionStatus>();
        }

        #region FromString()

        [Fact]
        public void FromString_TickerRawContentNull_ThrowsArgumentNullException()
        {
            Assert.Equal("rawMessage", 
                Assert.Throws<ArgumentNullException>(() => TickerMessage.CreateFromString(null, subscription.Object)).ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("dlasfjdaoifjaokdmfaoidja")]
        public void FromString_TickerRawContentValue_ThrowsArgumentOutOfRangeException(string value)
        {
            Assert.Equal("rawMessage",
                Assert.Throws<ArgumentOutOfRangeException>(() => TickerMessage.CreateFromString(value, subscription.Object)).ParamName);
        }

        [Fact]
        public void FromString_move()
        {
            TickerMessage.CreateFromString(TestSocketMessages.TickerMessage, new SubscriptionStatus()
            {
                ChannelId = 123,
                Pair = "XBT/EUR"
            });

        }

        #endregion
    }
}
