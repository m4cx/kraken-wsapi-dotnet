using System.Threading.Tasks;

namespace Kraken.WebSockets.Authentication
{
    public interface IAuthenticationClient
    {
        Task<AuthToken> GetWebsocketToken();
    }
}