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

        #endregion

    }
}
