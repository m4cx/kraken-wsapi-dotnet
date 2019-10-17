using System;
using Kraken.WebSockets.Authentication;
using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets
{
    public class KrakenApi : IKrakenApi
    {
        private AuthenticationClient authenticationClient;
        private string websocketUri;

        private static readonly Lazy<IKrakenApiClientFactory> factory =
            new Lazy<IKrakenApiClientFactory>(() => new KrakenApiClientFactory(new KrakenMessageSerializer()));

        public IKrakenApi ConfigureAuthentication(string uri, string apiKey, string apiSecret, int version = 0)
        {
            authenticationClient = new AuthenticationClient(uri, apiKey.ToSecureString(), apiSecret.ToSecureString(), version);

            return this;
        }

        public IKrakenApi ConfigureWebsocket(string uri)
        {
            websocketUri = uri;

            return this;
        }

        public IAuthenticationClient AuthenticationClient => 
            authenticationClient ?? throw new InvalidOperationException("Please configure authentication.");

        public IKrakenApiClient BuildClient()
        {
            return factory.Value.Create(websocketUri ?? throw new InvalidOperationException("Please configure websocket"));
        }
    }
}
