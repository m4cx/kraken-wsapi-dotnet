using Kraken.WebSockets.Authentication;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Authentication
{
    [ExcludeFromCodeCoverage]
    public class SecureStringExtensionsTests
    {
        public SecureStringExtensionsTests()
        {

        }

        #region ToSecureString()

        [Fact]
        public void ToSecureString_ValueNull_ThrowsArgumentNullException()
        {
            Assert.Equal("value",
                Assert.Throws<ArgumentNullException>(() => SecureStringExtensions.ToSecureString(null)).ParamName);
        }

        [Fact]
        public void ToSecureString_EmptyString_ReturnsSecureString()
        {
            var result = string.Empty.ToSecureString();
            Assert.NotNull(result);
            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void ToSecureString_TestString_ReturnsSecureString()
        {
            var result = "Test".ToSecureString();
            Assert.NotNull(result);
            Assert.Equal(4, result.Length);
        }

        #endregion

        #region ToPlainString() 

        [Fact]
        public void ToPlainString_ValueNull_ThrowsArgumentNullException()
        {
            Assert.Equal("value",
                Assert.Throws<ArgumentNullException>(() => SecureStringExtensions.ToPlainString(null)).ParamName);
        }

        [Fact]
        public void ToPlainString_EmptyString_ReturnsSecureString()
        {
            var result = string.Empty.ToSecureString().ToPlainString();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToPlainString_TestString_ReturnsSecureString()
        {
            var result = "Test".ToSecureString().ToPlainString();
            Assert.Equal("Test", result);
        }

        #endregion
    }
}
