using Kraken.WebSockets.Messages;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public class KrakenWebSocketTests
    {
        private readonly Mock<IKrakenMessageSerializer> serializer;
        private readonly KrakenWebSocket instance;

        public KrakenWebSocketTests()
        {
            serializer = new Mock<IKrakenMessageSerializer>();
            instance = new KrakenWebSocket("ws://localhost:29100", serializer.Object);
        }

        #region Ctor

        [Fact]
        public void Ctor_UriNull_ThrowsArgumentNullException()
        {
            Assert.Equal("uri", Assert.Throws<ArgumentNullException>(() => new KrakenWebSocket(null, serializer.Object)).ParamName);
        }

        [Fact]
        public void Ctor_SerializerNull_ThrowsArgumentNullException()
        {
            Assert.Equal("serializer", Assert.Throws<ArgumentNullException>(() => new KrakenWebSocket("ws://test.example.com", null)).ParamName);
        }

        [Fact]
        public void Ctor_InstanceCreated()
        {
            Assert.NotNull(instance);
        }

        #endregion
    }
}
