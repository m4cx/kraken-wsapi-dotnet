using Kraken.WebSockets.Converters;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    public class DecimalToStringConverterTests
    {
        private readonly DecimalToStringConverter instance;

        public DecimalToStringConverterTests()
        {
            instance = new DecimalToStringConverter();
        }

        #region CanConvert() 

        [Theory]
        [InlineData(typeof(decimal), true)]
        [InlineData(typeof(decimal?), true)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(object), false)]
        public void CanConvert_ChecksValues(Type objectType, bool result)
        {
            Assert.Equal(result, instance.CanConvert(objectType));
        }

        #endregion

        #region ReadJson()

        [Theory]
        [InlineData(@"{""Property"":""1""}")]
        [InlineData(@"{""Property"":""1.5""}")]
        public void ReadJson_WithTestClass_ReturnsDecimalValue(string json)
        {
            Assert.Throws<NotImplementedException>(() => JsonConvert.DeserializeObject<TestClass>(json));
        }

        #endregion

        #region TestClass

        private class TestClass
        {
            [JsonConverter(typeof(DecimalToStringConverter))]
            public decimal Property { get; set; }

            [JsonConverter(typeof(DecimalToStringConverter))]
            public decimal? NullableProperty { get; set; }
        }

        #endregion
    }
}
