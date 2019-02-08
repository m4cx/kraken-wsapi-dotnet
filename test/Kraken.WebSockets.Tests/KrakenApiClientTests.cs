using System;
using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using Moq;
using Xunit;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public class KrakenApiClientTests
    {
        private readonly Mock<IKrakenSocket> socket;
        private readonly Mock<IKrakenMessageSerializer> serializer;
        private readonly KrakenApiClient instance;

        public KrakenApiClientTests()
        {
            socket = new Mock<IKrakenSocket>();
            serializer = new Mock<IKrakenMessageSerializer>();
            instance = new KrakenApiClient(socket.Object, serializer.Object);
        }

        #region Ctor

        [Fact]
        public void Ctor_SocketNull_ThrowsArgumentNullException()
        {
            Assert.Equal("socket", 
                Assert.Throws<ArgumentNullException>(
                    () => new KrakenApiClient(null, serializer.Object)).ParamName);
        }
        
        [Fact]
        public void Ctor_SerializerNull_ThrowsArgumentNullException()
        {
            Assert.Equal("serializer", 
                Assert.Throws<ArgumentNullException>(
                    () => new KrakenApiClient(socket.Object, null)).ParamName);
        }

        #endregion
        
        #region SystemStatus

        [Fact]
        public void SystemStatus_Get_ReturnsNullAfterCreation()
        {
            Assert.Null(instance.SystemStatus);
        }

        [Fact]
        public void SystemStatus_Get_AfterMessageReceivedReturnsSentValue()
        {   
            serializer
                .Setup(x => x.Deserialize<SystemStatus>(It.Is<string>(y => y == TestSocketMessages.SystemStatusMessage)))
                .Returns(TestSocketMessages.SystemStatus);
            
            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("systemStatus", TestSocketMessages.SystemStatusMessage));
            
            Assert.Equal(TestSocketMessages.SystemStatus, instance.SystemStatus);
        }
        
        #endregion
    }
}
