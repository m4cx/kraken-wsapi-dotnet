using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Kraken.WebSockets.Authentication
{
    public interface IAuthenticationClientFactory
    {
        IAuthenticationClient Create(string uri, string apiKey, string apiSecret);
    }
}
