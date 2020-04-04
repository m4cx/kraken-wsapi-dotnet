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
    }
}
