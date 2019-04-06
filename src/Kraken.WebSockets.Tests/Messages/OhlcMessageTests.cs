using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class OhlcMessageTests
    {
        #region CreateFromString()

        [Fact]
        public void CreateFromString_ExampleOfPayload_ReturnsOhlcMessage()
        {
            var exampleOfPayload = @"[42,[""1542057314.748456"",""1542057360.435743"",""3586.70000"",""3586.70000"",""3586.60000"",""3586.60000"",""3586.68894"",""0.03373000"",2]]";
            var message = OhlcMessage.CreateFromString(exampleOfPayload);
            Assert.NotNull(message);

            Assert.Equal(42, message.ChannelId);
            Assert.Equal(1542057314.748456M, message.Time);
            Assert.Equal(1542057360.435743M, message.EndTime);
            Assert.Equal(3586.70000M, message.Open);
            Assert.Equal(3586.70000M, message.High);
            Assert.Equal(3586.60000M, message.Low);
            Assert.Equal(3586.60000M, message.Close);
            Assert.Equal(3586.68894M, message.Vwap);
            Assert.Equal(0.03373000M, message.Volume);
            Assert.Equal(2, message.Count);
        }

        #endregion
    }
}
