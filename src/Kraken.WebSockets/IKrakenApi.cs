using System.Security;
using Kraken.WebSockets.Authentication;

namespace Kraken.WebSockets
{
    public interface IKrakenApi
    {
        IAuthenticationClient AuthenticationClient { get; }

        IKrakenApi ConfigureAuthentication(string uri, string apiKey, string apiSecret, int version = 0);

        IKrakenApi ConfigureWebsocket(string uri);

        IKrakenApiClient BuildClient();
    }
}