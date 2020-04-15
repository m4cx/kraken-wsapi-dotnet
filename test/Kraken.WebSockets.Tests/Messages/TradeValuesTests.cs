using Kraken.WebSockets.Messages;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class TradeValuesTests
    {
        #region CreateFromJArray()

        [Fact]
        public void CreateFromJArray_Test()
        {
            var tradeValuesJArray = JArray.Parse(@"[""5541.20000"",""0.15850568"",""1534614057.321597"",""s"",""l"",""""]");
            var tradeValues = TradeValues.CreateFromJArray(tradeValuesJArray);

            Assert.NotNull(tradeValues);
            Assert.Equal(5541.20000M, tradeValues.Price);
            Assert.Equal(0.15850568M, tradeValues.Volume);
            Assert.Equal(1534614057.321597M, tradeValues.Time);
            Assert.Equal("s", tradeValues.Side);
            Assert.Equal("l", tradeValues.OrderType);
            Assert.Equal(string.Empty, tradeValues.Misc);
        }

        #endregion
    }
}
