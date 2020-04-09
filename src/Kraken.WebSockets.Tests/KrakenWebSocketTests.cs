using Kraken.WebSockets.Messages;
using Kraken.WebSockets.Sockets;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using Xunit;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public class KrakenWebSocketTests
    {
        private readonly Mock<IKrakenMessageSerializer> serializer;
        private readonly Mock<IWebSocket> webSocket;
        private readonly KrakenWebSocket instance;

        public KrakenWebSocketTests()
        {
            serializer = new Mock<IKrakenMessageSerializer>();
            webSocket = new Mock<IWebSocket>();
            instance = new KrakenWebSocket("ws://localhost:29100", serializer.Object, new DefaultWebSocket(new ClientWebSocket()));
        }

        #region Ctor

        [Fact]
        public void Ctor_UriNull_ThrowsArgumentNullException()
        {
            Assert.Equal("uri", Assert.Throws<ArgumentNullException>(() => new KrakenWebSocket(null, serializer.Object, webSocket.Object)).ParamName);
        }

        [Fact]
        public void Ctor_SerializerNull_ThrowsArgumentNullException()
        {
            Assert.Equal("serializer", Assert.Throws<ArgumentNullException>(() => new KrakenWebSocket("ws://test.example.com", null, webSocket.Object)).ParamName);
        }

        [Fact]
        public void Ctor_WebSocketNull_ThrowsArgumentNullException()
        {
            Assert.Equal("webSocket", Assert.Throws<ArgumentNullException>(() => new KrakenWebSocket("ws://test.example.com", serializer.Object, null)).ParamName);
        }

        [Fact]
        public void Ctor_InstanceCreated()
        {
            Assert.NotNull(instance);
        }

        #endregion
    }
}
