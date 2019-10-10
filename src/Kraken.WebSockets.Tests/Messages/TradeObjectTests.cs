using System;
using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Messages;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class TradeObjectTests
    {
        private JObject tradeJObject;
        private string tradeId;

        public TradeObjectTests()
        {
            tradeId = "TDLH43-DVQXD-2KHVYY";
            tradeJObject = JObject.Parse(@"{""cost"": ""1000000.00000"",""fee"": ""1600.00000"",""margin"": ""0.00000"",""ordertxid"": ""TDLH43-DVQXD-2KHVYY"",""ordertype"": ""limit""," +
                                         @"""pair"": ""XBT/EUR"",""postxid"": ""OGTT3Y-C6I3P-XRI6HX"",""price"": ""100000.00000"",""time"": ""1560520332.914657"",""type"": ""sell"",""vol"": ""1000000000.00000000""}");

        }

        #region CreateFromString() 

        [Fact]
        public void CreateFromString_TradeIdNull_ThrowsArgumentNullException()
        {
            Assert.Equal("tradeId", Assert.Throws<ArgumentNullException>(() => TradeObject.CreateFromJObject(null, tradeJObject)).ParamName);
        }

        [Fact]
        public void CreateFromString_JObjectdNull_ThrowsArgumentNullException()
        {
            Assert.Equal("jObject", Assert.Throws<ArgumentNullException>(() => TradeObject.CreateFromJObject(tradeId, null)).ParamName);
        }

        [Fact]
        public void CreateFromString_CreatesValidObject()
        {
            var trade = TradeObject.CreateFromJObject(tradeId, tradeJObject);

            Assert.NotNull((trade));
            Assert.Equal(1600M, trade.Fee);
            Assert.Equal(1000000M, trade.Cost);
        }

        #endregion
    }
}
