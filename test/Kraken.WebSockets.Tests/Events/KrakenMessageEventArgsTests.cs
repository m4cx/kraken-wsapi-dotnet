using System;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Events
{
    public class KrakenMessageEventArgsTests
    {
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
            testJson = @"{""event"":""Test""}";
            instance = new KrakenMessageEventArgs(testJson);
        }

        #region Ctor

        [Fact]
        public void Ctor_RawContentNull_ThrowsArgumentNullException()
        {
            Assert.Equal("rawContent",
                Assert.Throws<ArgumentNullException>(() => 
                    new KrakenMessageEventArgs(null)).ParamName);
        }

        [Fact]
        public void Ctor_RawContentEmptyString_ThrowsArgumentOutOfRangeException()
        {
            Assert.Equal("rawContent",
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    new KrakenMessageEventArgs(string.Empty)).ParamName);
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
