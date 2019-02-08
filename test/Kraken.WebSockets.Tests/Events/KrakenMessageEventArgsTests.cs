using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using Moq;
using Xunit;

namespace Kraken.WebSockets.Tests.Events
{
    public class KrakenMessageEventArgsTests
    {
        private readonly string eventString;
        private readonly string testJson;
        private KrakenMessageEventArgs instance;

        public KrakenMessageEventArgsTests()
        {
            eventString = "Test";
            testJson = @"{""event"":""Test""}";
            instance = new KrakenMessageEventArgs(eventString, testJson);
        }

        #region Ctor

        [Fact]
        public void Ctor_Default_InstanceIsNotNull()
        {
            Assert.NotNull(instance);
        }

        #endregion

        #region RawContent

        [Fact]
        public void RawContent_Get_ReturnsRawValueSet()
        {
            Assert.Equal(testJson, instance.RawContent);
        }

        #endregion

        #region RawContent

        [Fact]
        public void Event_Get_ReturnsRawValueSet()
        {
            Assert.Equal(eventString, instance.Event);
        }

        #endregion
    }
}
