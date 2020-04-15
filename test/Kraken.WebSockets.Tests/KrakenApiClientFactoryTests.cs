using Kraken.WebSockets.Messages;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public class KrakenApiClientFactoryTests
    {
        private Mock<IKrakenMessageSerializer> serializerMock;
        private KrakenApiClientFactory instance;

        public KrakenApiClientFactoryTests()
        {
            serializerMock = new Mock<IKrakenMessageSerializer>();
            instance = new KrakenApiClientFactory(serializerMock.Object);
        }

        #region Ctor

        [Fact]
        public void Ctor_SerializerNull_ThrowsArgumentNullException()
        {
            Assert.Equal("serializer",
                Assert.Throws<ArgumentNullException>(() => new KrakenApiClientFactory(null)).ParamName);
        }

        [Fact]
        public void Ctor_SerializerSet_ShouldCreateInstance()
        {
            Assert.NotNull(instance);
            Assert.IsType<KrakenApiClientFactory>(instance);
        }

        #endregion

        #region Create()

        [Fact]
        public void Create_UriNull_ThrowsArgumentNullException()
        {
            Assert.Equal("uri", Assert.Throws<ArgumentNullException>(() => instance.Create(null)).ParamName);
        }

        [Fact]
        public void Create_UriEmptyString_ThrowsArgumentOutOfRangeException()
        {
            Assert.Equal("uri", Assert.Throws<ArgumentOutOfRangeException>(() => instance.Create(string.Empty)).ParamName);
        }

        [Fact]
        public void Create_ShouldCreateClientInstance()
        {
            IKrakenApiClient client = instance.Create("wss://ws-beta.kraken.com");
            Assert.NotNull(client);
            Assert.IsAssignableFrom<IKrakenApiClient>(client);
        }

        #endregion
    }
}
