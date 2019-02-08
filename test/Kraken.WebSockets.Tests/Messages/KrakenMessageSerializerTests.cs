using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class KrakenMessageSerializerTests
    {
        private readonly KrakenMessageSerializer instance;

        public KrakenMessageSerializerTests()
        {
            instance = new KrakenMessageSerializer();
        }

        #region Serialize()

        [Fact]
        public void Serialize_Null_ThrowsArgumentNullException()
        {
            Assert.Equal("message",
            Assert.Throws<ArgumentNullException>(() => instance.Serialize<KrakenMessage>(null)).ParamName);
        }
        
        [Fact]
        public void Serialize_PingMessage_ReturnsPingMessageJson()
        {
            Assert.Equal(TestSocketMessages.PingMessage, instance.Serialize(TestSocketMessages.Ping));
        }

        #endregion

        #region Deserialize()

        [Fact]
        public void Deserialize_Null_ThrowsArgumentNullException()
        {
            Assert.Equal("json",
                Assert.Throws<ArgumentNullException>(() => instance.Deserialize<KrakenMessage>(null)).ParamName);
        }
        
        [Fact]
        public void Deserialize_StringEmpty_ThrowsArgumentNullException()
        {
            Assert.Equal("json",
                Assert.Throws<ArgumentNullException>(() => instance.Deserialize<KrakenMessage>(string.Empty)).ParamName);
        }
        
        [Fact]
        public void Deserialize_SystemStatusMessage_ReturnsSystemStatus()
        {
            var result = instance.Deserialize<SystemStatus>(TestSocketMessages.SystemStatusMessage);
            Assert.Equal(TestSocketMessages.SystemStatus.Event, result.Event);
            Assert.Equal(TestSocketMessages.SystemStatus.Status, result.Status);
            Assert.Equal(TestSocketMessages.SystemStatus.Version, result.Version);
            Assert.Equal(TestSocketMessages.SystemStatus.ConnectionId, result.ConnectionId);
        }

        #endregion
    }
}
