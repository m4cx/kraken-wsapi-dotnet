using Kraken.WebSockets.Messages;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class SubscribeTests
    {
        private const int RequestId = 123;
        private readonly string[] pairs;
        private readonly SubscribeOptions options;
        private readonly Subscribe instance;

        public SubscribeTests()
        {
            pairs = new[] { "XBT/EUR" };
            options = new SubscribeOptions("*") { };
            instance = new Subscribe(pairs, options, RequestId);
        }

        #region Ctor

        [Fact(Skip = "Transform nullable pairs to separate subscription")]
        public void Ctor_PairsNull_ThrowsArgumentNullException()
        {
            Assert.Equal("pairs", Assert.Throws<ArgumentNullException>(() => new Subscribe(null, options, RequestId)).ParamName);
        }

        [Fact]
        public void Ctor_OptionsNull_ThrowsArgumentNullException()
        {
            Assert.Equal("options", Assert.Throws<ArgumentNullException>(() => new Subscribe(pairs, null, RequestId)).ParamName);
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
            Assert.Equal(RequestId, instance.RequestId);
        }

        #endregion
    }
}
