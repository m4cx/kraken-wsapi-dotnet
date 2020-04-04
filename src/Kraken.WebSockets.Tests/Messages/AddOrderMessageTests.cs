using Kraken.WebSockets.Messages;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class AddOrderMessageTests 
    {
        private readonly string token;
        private readonly OrderType orderType;
        private readonly Side side;
        private readonly string pair;
        private readonly decimal volume;

        public AddOrderMessageTests()
        {
            token = "token";
            orderType = OrderType.Limit;
            side = Side.Sell;
            pair = "XBT/EUR";
            volume = 1M;
        }

        #region Ctor

        [Fact]
        public void Ctor_TokenNull_ThrowsArgumentNullExcetion()
        {
            Assert.Equal("token", Assert.Throws<ArgumentNullException>(() => new AddOrderCommand(null, orderType, side, pair, volume)).ParamName);
        }

        [Fact]
        public void Ctor_PairNull_ThrowsArgumentNullExcetion()
        {
            Assert.Equal("pair", Assert.Throws<ArgumentNullException>(() => new AddOrderCommand(token, orderType, side, null, volume)).ParamName);
        }

        #endregion
    }
}
