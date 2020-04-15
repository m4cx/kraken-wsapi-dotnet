using Kraken.WebSockets.Converters;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    public class StatusConverterTests
    {
        private readonly StatusConverter instance;

        public StatusConverterTests()
        {
            instance = new StatusConverter();
        }

        #region CanConvert() 

        [Theory]
        [InlineData(typeof(Status), true)]
        [InlineData(typeof(Status?), true)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(int?), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(object), false)]
        public void CanConvert_ChecksValues(Type objectType, bool result)
        {
            Assert.Equal(result, instance.CanConvert(objectType));
        }

        #endregion

        #region WriteJson()

        [Theory]
        [InlineData(Status.Ok, null)]
        [InlineData(Status.Error, null)]
        [InlineData(Status.Ok, Status.Ok)]
        [InlineData(Status.Ok, Status.Error)]
        public void WriteJson_ThrowsNotImplementedException(Status property, Status? nullableProperty)
        {
            Assert.Throws<NotImplementedException>(() => JsonConvert.SerializeObject(new TestClass
            {
                Property = property,
                NullableProperty = nullableProperty
            }));
        }

        #endregion

        #region TestClass

        private class TestClass
        {
            [JsonConverter(typeof(StatusConverter))]
            public Status Property { get; set; }

            [JsonConverter(typeof(StatusConverter))]
            public Status? NullableProperty { get; set; }
        }

        #endregion
    }
}
