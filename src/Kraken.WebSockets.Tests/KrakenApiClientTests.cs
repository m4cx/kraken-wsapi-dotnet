using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Logging;
using Kraken.WebSockets.Messages;
using Microsoft.Extensions.Logging.Abstractions;
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
            LogManager.LoggerFactory = new NullLoggerFactory();

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

        #region ConnectAsync()

        [Fact]
        public async Task ConnectAsync_ShouldCallSocketConnectAsync()
        {
            await instance.ConnectAsync();

            socket.Verify(x => x.ConnectAsync());
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
            socket.Verify(mock => mock.SendAsync(It.Is<Subscribe>(x => x == subscribe)), Times.Once);
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

        #region KrakenDataMessageHandling

        [Fact]
        public void KrakenDataMessage_TickerIsReceivedAndPropagatedThoughEvent()
        {
            instance.Subscriptions.Add(0, new SubscriptionStatus() { ChannelId = 0, Pair = "EUR/XBT", Subscription = new SubscribeOptions(SubscribeOptionNames.Ticker) });
            bool handlerExecuted = false;
            instance.TickerReceived += (sender, e) =>
            {
                Assert.Equal(0, e.ChannelId);
                Assert.Equal("EUR/XBT", e.Pair);
                Assert.IsType<TickerMessage>(e.DataMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null, new KrakenMessageEventArgs("data", TestSocketMessages.TickerMessage, 0));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_OhlcIsReceivedAndPropagatedThroughEvent()
        {
            const int expectedChannelId = 42;
            instance.Subscriptions.Add(expectedChannelId, new SubscriptionStatus() { ChannelId = expectedChannelId, Pair = "EUR/XBT", Subscription = new SubscribeOptions(SubscribeOptionNames.OHLC) });
            bool handlerExecuted = false;
            instance.OhlcReceived += (sender, e) =>
            {
                Assert.Equal(expectedChannelId, e.ChannelId);
                Assert.Equal("EUR/XBT", e.Pair);
                Assert.IsType<OhlcMessage>(e.DataMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null, new KrakenMessageEventArgs("data", TestSocketMessages.OhlcMessage, expectedChannelId));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_TradeIsReceivedAndPropagatedThroughEvent()
        {
            const int expectedChannelId = 0;
            instance.Subscriptions.Add(expectedChannelId, new SubscriptionStatus() { ChannelId = expectedChannelId, Pair = "EUR/XBT", Subscription = new SubscribeOptions(SubscribeOptionNames.Trade) });
            bool handlerExecuted = false;
            instance.TradeReceived += (sender, e) =>
            {
                Assert.Equal(expectedChannelId, e.ChannelId);
                Assert.Equal("EUR/XBT", e.Pair);
                Assert.IsType<TradeMessage>(e.DataMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null, new KrakenMessageEventArgs("data", TestSocketMessages.TradeMessage, expectedChannelId));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_SpreadIsReceivedAndPropagatedThroughEvent()
        {
            const int expectedChannelId = 0;
            instance.Subscriptions.Add(
                expectedChannelId,
                new SubscriptionStatus()
                {
                    ChannelId = expectedChannelId,
                    Pair = "EUR/XBT",
                    Subscription = new SubscribeOptions(SubscribeOptionNames.Spread)
                });
            bool handlerExecuted = false;
            instance.SpreadReceived += (sender, e) =>
            {
                Assert.Equal(expectedChannelId, e.ChannelId);
                Assert.Equal("EUR/XBT", e.Pair);
                Assert.IsType<SpreadMessage>(e.DataMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("data", TestSocketMessages.SpreadMessage, expectedChannelId));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_BookSnapshotIsReceivedAndPropagatedThroughEvent()
        {
            const int expectedChannelId = 0;
            instance.Subscriptions.Add(
                expectedChannelId,
                new SubscriptionStatus()
                {
                    ChannelId = expectedChannelId,
                    Pair = "EUR/XBT",
                    Subscription = new SubscribeOptions(SubscribeOptionNames.Book)
                });
            bool handlerExecuted = false;
            instance.BookSnapshotReceived += (sender, e) =>
            {
                Assert.Equal(expectedChannelId, e.ChannelId);
                Assert.Equal("EUR/XBT", e.Pair);
                Assert.IsType<BookSnapshotMessage>(e.DataMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("data", TestSocketMessages.BookSnapshotMessage, expectedChannelId));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_BookUpdateWithCompleteDataIsReceivedAndPropagatedThroughEvent()
        {
            const int expectedChannelId = 1234;
            instance.Subscriptions.Add(
                expectedChannelId,
                new SubscriptionStatus()
                {
                    ChannelId = expectedChannelId,
                    Pair = "EUR/XBT",
                    Subscription = new SubscribeOptions(SubscribeOptionNames.Book)
                });
            bool handlerExecuted = false;
            instance.BookUpdateReceived += (sender, e) =>
            {
                Assert.Equal(expectedChannelId, e.ChannelId);
                Assert.Equal("EUR/XBT", e.Pair);
                Assert.IsType<BookUpdateMessage>(e.DataMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("data", TestSocketMessages.BookUpdateCompleteMessage, expectedChannelId));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_HeartbeatIsReceivedAndPropagatedThroughEvent()
        {
            bool handlerExecuted = false;
            instance.HeartbeatReceived += (sender, e) =>
            {
                Assert.Null(e.Message);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("heartbeat", @"{""event"":""heartbeat""}"));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_OwnTradesIsReceivedAndPropagatedThroughEvent()
        {
            bool handlerExecuted = false;
            instance.OwnTradesReceived += (sender, args) =>
            {
                Assert.IsType<OwnTradesMessage>(args.PrivateMessage);
                handlerExecuted = true;
            };

            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("private", TestSocketMessages.OwnTradesMessage));
            Assert.True(handlerExecuted);
        }

        [Fact]
        public void KrakenDataMessage_UnknownDataNoEventIsEmitted()
        {
            bool handlerExecuted = false;
            Action handler = () => handlerExecuted = true;
            instance.BookSnapshotReceived += (sender, e) => handler();
            instance.BookUpdateReceived += (sender, e) => handler();
            instance.TradeReceived += (sender, e) => handler();
            instance.SpreadReceived += (sender, e) => handler();
            instance.OhlcReceived += (sender, e) => handler();
            instance.TradeReceived += (sender, e) => handler();

            socket.Raise(x => x.DataReceived += null,
                new KrakenMessageEventArgs("data", TestSocketMessages.BookUpdateCompleteMessage, 1));
            Assert.False(handlerExecuted);
        }

        #endregion

        #region Dispose()
        [Fact]
        public void Dispose_ShouldCloseSocket()
        {
            instance.Dispose();
            socket.Verify(s => s.CloseAsync());
        }

        [Fact]
        public void Dispose_ShouldUnsubscribeFromExistingSubscriptions()
        {
            serializer
                .Setup(x => x.Deserialize<SubscriptionStatus>(It.Is<string>(y => y == TestSocketMessages.SubscriptionStatus1Message)))
                .Returns(TestSocketMessages.SubscriptionStatus1);

            socket.Raise(x => x.DataReceived += null, new KrakenMessageEventArgs(SubscriptionStatus.EventName, TestSocketMessages.SubscriptionStatus1Message));

            instance.Dispose();
            socket.Verify(s => s.SendAsync(It.Is<Unsubscribe>(u => u.ChannelId == 123)));
        }

        [Fact]
        public void Dispose_ShouldCloseSocketOnlyOnce()
        {
            instance.Dispose();
            instance.Dispose();

            socket.Verify(s => s.CloseAsync(), Times.Once);
        }

        #endregion
    }
}
