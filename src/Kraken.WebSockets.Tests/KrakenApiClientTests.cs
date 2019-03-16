using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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

        #region SubscribeAsync()

        [Fact]
        public async Task SubscribeAsync_SubscribeNull_ThrowsArgumentNullException() 
            => Assert.Equal("subscribe", (await Assert.ThrowsAsync<ArgumentNullException>(() => instance.SubscribeAsync(null))).ParamName);

        [Fact]
        public async Task SubscribeAsync_WithSubscribe_SubscribeIsPassedToSocket()
        {
            var subscribe = new Subscribe(new string[] { "XBT/EUR" }, new SubscribeOptions(SubscribeOptionNames.All));
            await instance.SubscribeAsync(subscribe);
            socket.Verify( mock => mock.SendAsync(It.Is<Subscribe>(x => x == subscribe)), Times.Once);
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
                new KrakenMessageEventArgs(SystemStatus.EventName, TestSocketMessages.SystemStatusMessage));

            Assert.Equal(TestSocketMessages.SystemStatus, instance.SystemStatus);
        }

        #endregion

        #region SubscriptionStatus

        [Fact]
        public void SubscriptionStatus_Get_AtStartup_ReturnsEmptyDictionary()
        {
            Assert.Empty(instance.Subscriptions);
        }

        [Fact]
        public void SubscriptionStatus_Get_AfterMessageReceived_ReturnsSingleSubscription()
        {
            serializer
                .Setup(x => x.Deserialize<SubscriptionStatus>(It.Is<string>(y => y == TestSocketMessages.SubscriptionStatus1Message)))
                .Returns(TestSocketMessages.SubscriptionStatus1);

            socket.Raise(x => x.DataReceived += null, new KrakenMessageEventArgs(SubscriptionStatus.EventName, TestSocketMessages.SubscriptionStatus1Message));
            
            Assert.Equal(1, instance.Subscriptions.Count);
            Assert.Equal(TestSocketMessages.SubscriptionStatus1, instance.Subscriptions[TestSocketMessages.SubscriptionStatus1.ChannelId.Value]);
        }

        [Fact]
        public void SubscriptionStatus_Get_AfterMessageWithoutChannelIdReceived_ReturnsNoSubscription()
        {
            serializer
                .Setup(x => x.Deserialize<SubscriptionStatus>(It.Is<string>(y => y == TestSocketMessages.SubScriptionStatusNoChannelIdMessage)))
                .Returns(TestSocketMessages.SubScriptionStatusNoChannelId);

            socket.Raise(x => x.DataReceived += null, new KrakenMessageEventArgs(SubscriptionStatus.EventName, TestSocketMessages.SubScriptionStatusNoChannelIdMessage));

            Assert.Equal(0, instance.Subscriptions.Count);
        }


        #endregion

        #region UnsubscribeAsync()

        [Fact]
        public async Task UnsubscribeAsync_ChannelIdZero_ThrowsArgumentOutOfRangeException() 
            => Assert.Equal("channelId", (await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => instance.UnsubscribeAsync(0))).ParamName);

        [Fact]
        public async Task UnsubscribeAsync_SubscriptionWithChannelId_ThrowsArgumentNullException()
        {
            await instance.UnsubscribeAsync(123);
            socket.Verify(x => x.SendAsync(It.Is<Unsubscribe>(y => y.ChannelId == 123)));
        }

        #endregion
    }
}
