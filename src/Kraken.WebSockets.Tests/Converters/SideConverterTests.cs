using Kraken.WebSockets.Converters;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    public class SideConverterTests
    {
        private readonly SideConverter instance;

        public SideConverterTests()
        {
            instance = new SideConverter();
        }

        #region CanConvert() 

        [Theory]
        [InlineData(typeof(Side), true)]
        [InlineData(typeof(Side?), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(object), false)]
        public void CanConvert_ChecksValues(Type objectType, bool result)
        {
            Assert.Equal(result, instance.CanConvert(objectType));
        }

        #endregion

        #region ReadJson()

        [Theory]
        [InlineData(@"{""Property"":""buy""}")]
        [InlineData(@"{""Property"":""sell""}")]
        public void ReadJson_WithTestClass_ThrowsNotImplementedException(string json)
        {
            Assert.Throws<NotImplementedException>(() => JsonConvert.DeserializeObject<TestClass>(json));
        }

        #endregion

        #region TestClass

        private class TestClass
        {
            [JsonConverter(typeof(SideConverter))]
            public Side Property { get; set; }

            [JsonConverter(typeof(SideConverter))]
            public Side? NullableProperty { get; set; }
        }

        #endregion
    }
}
