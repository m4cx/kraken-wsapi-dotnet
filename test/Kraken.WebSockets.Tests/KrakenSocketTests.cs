using Kraken.WebSockets.Logging;
using Kraken.WebSockets.Messages;
using Kraken.WebSockets.Sockets;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public class KrakenSocketTests
    {
        private readonly string uri;
        private readonly Mock<IKrakenMessageSerializer> serializer;
        private readonly Mock<IWebSocket> webSocket;
        private readonly KrakenSocket instance;

        public KrakenSocketTests()
        {
            LogManager.LoggerFactory = new NullLoggerFactory();
            uri = "ws://localhost:29100";
            serializer = new Mock<IKrakenMessageSerializer>();
            webSocket = new Mock<IWebSocket>();
            instance = new KrakenSocket(uri, serializer.Object, webSocket.Object);
        }

        #region Ctor

        [Fact]
        public void Ctor_UriNull_ThrowsArgumentNullException()
        {
            Assert.Equal("uri", Assert.Throws<ArgumentNullException>(() => new KrakenSocket(null, serializer.Object, webSocket.Object)).ParamName);
        }

        [Fact]
        public void Ctor_SerializerNull_ThrowsArgumentNullException()
        {
            Assert.Equal("serializer", Assert.Throws<ArgumentNullException>(() => new KrakenSocket("ws://test.example.com", null, webSocket.Object)).ParamName);
        }

        [Fact]
        public void Ctor_WebSocketNull_ThrowsArgumentNullException()
        {
            Assert.Equal("webSocket", Assert.Throws<ArgumentNullException>(() => new KrakenSocket("ws://test.example.com", serializer.Object, null)).ParamName);
        }

        [Fact]
        public void Ctor_InstanceCreated()
        {
            Assert.NotNull(instance);
        }

        #endregion

        #region ConnectAsync()

        [Fact]
        public async Task ConnectAsync_SuccessfullyConnects()
        {
            var executed = false;
            instance.Connected += (s, e) =>
            {
                executed = true;
            };

            await instance.ConnectAsync();

            webSocket.Verify(x => x.ConnectAsync(It.IsAny<Uri>(), default)); // TODO: Check correct uri
            Assert.True(executed);
        }

        [Fact]
        public async Task ConnectAsync_RethrowsExceptionOnInternalError()
        {
            var executed = false;
            instance.Connected += (s, e) =>
            {
                executed = true;
            };
            var exception = new Exception();

            webSocket.Setup(x => x.ConnectAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>())).Throws(exception);

            Assert.Equal(exception, await Assert.ThrowsAsync<Exception>(() => instance.ConnectAsync()));
            Assert.False(executed);
        }

        #endregion

        #region CloseAsync()

        [Theory]
        [InlineData(WebSocketState.Aborted)]
        [InlineData(WebSocketState.Closed)]
        [InlineData(WebSocketState.CloseReceived)]
        [InlineData(WebSocketState.CloseSent)]
        [InlineData(WebSocketState.Connecting)]
        public async Task CloseAsync_StateNotOpen_DoesNotPassThrough(WebSocketState state)
        {
            webSocket.Setup(x => x.State).Returns(state);
            await instance.CloseAsync();

            webSocket.Verify(x => x.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(WebSocketState.Open)]
        public async Task CloseAsync_StateOpen_DoesPassThrough(WebSocketState state)
        {
            webSocket.Setup(x => x.State).Returns(state);
            await instance.CloseAsync();

            webSocket.Verify(x => x.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region SendAsync()

        [Theory]
        [InlineData(WebSocketState.Aborted)]
        [InlineData(WebSocketState.Closed)]
        [InlineData(WebSocketState.CloseReceived)]
        [InlineData(WebSocketState.CloseSent)]
        [InlineData(WebSocketState.Connecting)]
        public async Task SendAsync_WithStateNotOpen_DoesNotPassThrough(WebSocketState state)
        {
            webSocket.Setup(x => x.State).Returns(state);

            await instance.SendAsync(new Ping());

            webSocket.Verify(x =>
                x.SendAsync(
                    It.IsAny<ArraySegment<byte>>(), 
                    It.IsAny<WebSocketMessageType>(), 
                    It.IsAny<bool>(), 
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(WebSocketState.Open)]
        public async Task SendAsync_WithStateOpen_DoesPassThrough(WebSocketState state)
        {
            webSocket.Setup(x => x.State).Returns(state);
            serializer.Setup(x => x.Serialize(It.IsAny<Ping>())).Returns(@"{""event"":""ping""}");

            await instance.SendAsync(new Ping());

            webSocket.Verify(x =>
                x.SendAsync(
                    It.IsAny<ArraySegment<byte>>(),
                    It.IsAny<WebSocketMessageType>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion
    }
}
