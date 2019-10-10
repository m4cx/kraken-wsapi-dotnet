using System.Diagnostics.CodeAnalysis;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public class KrakenApiTests
    {
        #region ClientFactory 

        [Fact]
        public void ClientFactory_Get_ReturnsInstance()
        {
            Assert.IsType<KrakenApiClientFactory>(KrakenApi.ClientFactory);
        }

        #endregion
    }
}
