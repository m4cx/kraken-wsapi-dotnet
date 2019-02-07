using System;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using Moq;
using Xunit;

namespace Kraken.WebSockets.Tests.Events
{
    public class KrakenMessageEventArgsTests
    {
        private readonly Mock<IKrakenMessageSerializer> serializer;
        private readonly string testJson;
        private KrakenMessageEventArgs instance;

        #region TestMessage

        private class TestMessage : KrakenMessage
        {
            public TestMessage() : base("Test")
            {
            }
        }

        #endregion

        public KrakenMessageEventArgsTests()
        {
            serializer = new Mock<IKrakenMessageSerializer>();
            testJson = @"{""event"":""Test""}";
            instance = new KrakenMessageEventArgs(testJson, serializer.Object);
        }

        #region Ctor

        [Fact]
        public void Ctor_RawContentNull_ThrowsArgumentNullException()
        {
            Assert.Equal("rawContent",
                Assert.Throws<ArgumentNullException>(() => 
                    new KrakenMessageEventArgs(null, serializer.Object)).ParamName);
        }

        [Fact]
        public void Ctor_RawContentEmptyString_ThrowsArgumentOutOfRangeException()
        {
            Assert.Equal("rawContent",
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    new KrakenMessageEventArgs(string.Empty, serializer.Object)).ParamName);
        }

        #endregion

        #region RawContent

        [Fact]
        public void RawContent_Get_ReturnsRawValueSet()
        {
            Assert.Equal(testJson, instance.RawContent);
        }

        #endregion
    }
}
