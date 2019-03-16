using Kraken.WebSockets.Messages;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class SubscribeTests
    {
        private const int requestId = 123;
        private readonly string[] pairs;
        private readonly SubscribeOptions options;
        private readonly Subscribe instance;

        public SubscribeTests()
        {

            pairs = new string[] { "XBT/EUR" };
            options = new SubscribeOptions("*") { };
            instance = new Subscribe(pairs, options, requestId);
        }

        #region Ctor

        [Fact]
        public void Ctor_PairsNull_ThrowsArgumentNullException()
        {
            Assert.Equal("pairs", Assert.Throws<ArgumentNullException>(() => new Subscribe(null, options, requestId)).ParamName);
        }

        [Fact]
        public void Ctor_OptionsNull_ThrowsArgumentNullException()
        {
            Assert.Equal("options", Assert.Throws<ArgumentNullException>(() => new Subscribe(pairs, null, requestId)).ParamName);
        }

        #endregion

        #region Pairs

        [Fact]
        public void Pairs_Get_ReturnsValue()
        {
            Assert.Equal(pairs, instance.Pairs);
        }

        #endregion

        #region Options

        [Fact]
        public void Options_Get_ReturnsValue()
        {
            Assert.Equal(options, instance.Options);
        }

        #endregion

        #region Options

        [Fact]
        public void RequestId_Get_ReturnsValue()
        {
            Assert.Equal(requestId, instance.RequestId);
        }

        #endregion
    }
}
