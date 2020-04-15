namespace Kraken.WebSockets.Authentication
{
    public class AuthToken
    {
        public string Token{ get; set; }

        public decimal? Expires { get; set; }
    }
}