using System;
using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class SubscribeOptionsTests
    {
        #region Ctor

        [Fact]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.Equal("name", 
                Assert.Throws<ArgumentNullException>(
                    () => new SubscribeOptions(null)).ParamName);
        }


        [Theory]
        [InlineData("")]
        [InlineData("test")]
        [InlineData("sldhfkasjhdfaksjdflkadslaf")]
        public void Ctor_ValueNotAllowed_ThrowsArgumentOutOfRangeException(string value)
        {
            Assert.Equal("name",
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => new SubscribeOptions(value)).ParamName);
        }

        [Theory]
        [InlineData(SubscribeOptionNames.All)]
        [InlineData(SubscribeOptionNames.Book)]
        [InlineData(SubscribeOptionNames.OHLC)]
        [InlineData(SubscribeOptionNames.Spread)]
        [InlineData(SubscribeOptionNames.Ticker)]
        [InlineData(SubscribeOptionNames.Trade)]
        public void Ctor_ValueAllowed_CreatesInstanceWithName(string value)
        {
            Assert.Equal(value, new SubscribeOptions(value).Name);
        }

        [Theory]
        [InlineData(SubscribeOptionNames.OwnTrades, null)]
        [InlineData(SubscribeOptionNames.OwnTrades, "")]
        public void Ctor_ValuePrivateTokenEmpty_ThrowsArgumentNullException(string name, string token)
        {
            Assert.Equal("token", Assert.Throws<ArgumentNullException>(() => new SubscribeOptions(name)).ParamName);
        }

        [Theory]
        [InlineData(SubscribeOptionNames.OwnTrades, "DF12A645-505C-46F9-BCCD-0F99CF7C8412")]
        public void Ctor_ValuePrivateTokenValue_CreatesInstance(string name, string token)
        {
            var instance = new SubscribeOptions(name, token);
            Assert.Equal(name, instance.Name);
            Assert.Equal(token, instance.Token);
        }

        #endregion

    }
}
