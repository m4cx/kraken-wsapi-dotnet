using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Kraken.WebSockets.Messages;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class OpenOrdersMessageTests
    {
        #region CreateFromString()

        [Fact]
        public void CreateFromString_Null_ThrowsArgumentNullException()
        {
            Assert.Equal("rawMessage",
                Assert.Throws<ArgumentNullException>(
                    () => OpenOrdersMessage.CreateFromString(null)).ParamName);
        }

        [Fact]
        public void CreateFromString_ValidMessageString_ReturnsMessageObject()
        {
            var instance = OpenOrdersMessage.CreateFromString(TestSocketMessages.OpenOrdersMessage);
            Assert.NotNull(instance);
            Assert.IsType<OpenOrdersMessage>(instance);
            Assert.Equal("openOrders", instance.ChannelName);
            Assert.Equal(4, instance.Orders.Count());
        }

        #endregion
    }
}
