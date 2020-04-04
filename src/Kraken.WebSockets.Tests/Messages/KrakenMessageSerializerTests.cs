using System;
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
        public void Serialize_PingMessage_ReturnsPingMessageJson() =>
            Assert.Equal(TestSocketMessages.PingMessage, instance.Serialize(TestSocketMessages.Ping));

        [Fact]
        public void Serialize_SubsribeMessage_ReturnsSubscribeJsonWithoutNullvalues() =>
            Assert.Equal(@"{""pair"":[""XBT/EUR""],""subscription"":{""name"":""*""},""event"":""subscribe""}",
                instance.Serialize(new Subscribe(new string[] { "XBT/EUR" }, new SubscribeOptions(SubscribeOptionNames.All))));

        #region AddOrderMessage

        [Fact]
        public void Serialize_AddOrderMessage()
        {
            var addOrder = new AddOrderCommand("0000000000000000000000000000000000000000", OrderType.Limit, Side.Buy, "XBT/USD", 10)
            {
                RequestId = 123,

                Price = 123.5M,
                Price2 = 125M,
                Leverage = 123.456M,
                Oflags = "viqc,fcib",
                Starttm = "+100",
                Expiretm = "+150",
                Userref = "123456",
                Validate = "true",
                CloseOrderType = OrderType.Market,
                ClosePrice = 123.5M,
                ClosePrice2 = 125M,
                TradingAgreement = "agree"
            };

            var addOrderString = instance.Serialize(addOrder);

            Assert.Contains(@"""event"":""addOrder""", addOrderString);
            Assert.Contains(@"""token"":""0000000000000000000000000000000000000000""", addOrderString);
            Assert.Contains(@"""reqid"":123", addOrderString);
            Assert.Contains(@"""ordertype"":""limit""", addOrderString);
            Assert.Contains(@"""type"":""buy""", addOrderString);
            Assert.Contains(@"""pair"":""XBT/USD""", addOrderString);
            Assert.Contains(@"""price"":""123.5""", addOrderString);
            Assert.Contains(@"""price2"":""125""", addOrderString);
            Assert.Contains(@"""volume"":""10""", addOrderString);
            Assert.Contains(@"""leverage"":""123.456""", addOrderString);
            Assert.Contains(@"""oflags"":""viqc,fcib""", addOrderString);
            Assert.Contains(@"""starttm"":""+100""", addOrderString);
            Assert.Contains(@"""expiretm"":""+150""", addOrderString);
            Assert.Contains(@"""userref"":""123456""", addOrderString);
            Assert.Contains(@"""validate"":""true""", addOrderString);
            Assert.Contains(@"""close[ordertype]"":""market""", addOrderString);
            Assert.Contains(@"""close[price]"":""123.5""", addOrderString);
            Assert.Contains(@"""close[price2]"":""125""", addOrderString);
            Assert.Contains(@"""trading_agreement"":""agree""", addOrderString);
        }

        #endregion

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

        [Fact]
        public void Deserialize_SubscriptionStatus1_ReturnsExpectedObjectStructure()
        {
            var result = instance.Deserialize<SubscriptionStatus>(TestSocketMessages.SubscriptionStatus1Message);
            Assert.Equal(TestSocketMessages.SubscriptionStatus1.Event, result.Event);
            Assert.Equal(TestSocketMessages.SubscriptionStatus1.Status, result.Status);
            Assert.Equal(TestSocketMessages.SubscriptionStatus1.Pair, result.Pair);
            Assert.Equal(TestSocketMessages.SubscriptionStatus1.ChannelId, result.ChannelId);
        }

        #region Heartbeat

        [Fact]
        public void Deserialize_Heartbeat_ReturnsExpectedObjectStructure()
        {
            var result = instance.Deserialize<Heartbeat>(TestSocketMessages.Heartbeat);
            Assert.IsType<Heartbeat>(result);
        }

        #endregion

        #region AddOrderStatus

        [Fact]
        public void Deserialize_AddOrderStatusSuccess_ReturnsObject()
        {
            var result = instance.Deserialize<AddOrderStatusEvent>(TestSocketMessages.AddOrderStatus);

            Assert.Equal("buy 0.01770000 XBTUSD @ limit 4000", result.Description);
            Assert.Equal("addOrderStatus", result.Event);
            Assert.Equal(Status.Ok, result.Status);
            Assert.Equal("ONPNXH-KMKMU-F4MR5V", result.OrderId);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.RequestId);
        }

        [Fact]
        public void Deserialize_AddOrderStatusError_ReturnsObject()
        {
            var result = instance.Deserialize<AddOrderStatusEvent>(TestSocketMessages.AddOrderStatusError);

            Assert.Equal("addOrderStatus", result.Event);
            Assert.Equal(Status.Error, result.Status);
            Assert.Equal("EOrder:Order minimum not met", result.ErrorMessage);
            Assert.Null(result.Description);
            Assert.Null(result.OrderId);
            Assert.Null(result.RequestId);
        }

        #endregion

        #endregion
    }
}
