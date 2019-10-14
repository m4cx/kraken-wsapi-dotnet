using System.Threading.Tasks;

namespace Kraken.WebSockets.Authentication
{
    internal interface IAuthenticationClient
    {
        Task<AuthToken> GetWebsocketToken();
    }
}