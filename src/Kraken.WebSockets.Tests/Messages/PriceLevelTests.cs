using Kraken.WebSockets.Messages;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class PriceLevelTests
    {
        #region CreateFromJArray()

        [Fact]
        public void CreateFromJArray_Test()
        {
            var priceLevelTokens = JArray.Parse(@"[""5541.30000"",""2.50700000"",""1534614248.123678""]");
            var priceLevel = PriceLevel.CreateFromJArray(priceLevelTokens);

            Assert.NotNull(priceLevel);
            Assert.Equal(5541.30000M, priceLevel.Price);
            Assert.Equal(2.50700000M, priceLevel.Volume);
            Assert.Equal(1534614248.123678M, priceLevel.Timestamp);
        }

        #endregion
    }
}
