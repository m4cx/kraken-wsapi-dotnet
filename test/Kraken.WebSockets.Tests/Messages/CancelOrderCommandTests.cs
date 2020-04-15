using Kraken.WebSockets.Messages;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public sealed class CancelOrderCommandTests
    {
        private readonly string token;
        private readonly string[] transactions;

        public CancelOrderCommandTests()
        {
            token = "00000000000";
            transactions = new[] { "ID1", "ID2" };
        }

        #region Ctor

        [Fact]
        public void Ctor_TokenNull_ThrowsArgumentNullExcetion()
        {
            Assert.Equal("token", Assert.Throws<ArgumentNullException>(() => new CancelOrderCommand(null, transactions)).ParamName);
        }

        [Fact]
        public void Ctor_TransactionsNull_ThrowsArgumentNullExcetion()
        {
            Assert.Equal("transactions", Assert.Throws<ArgumentNullException>(() => new CancelOrderCommand(token, null)).ParamName);
        }

        #endregion
    }
}
