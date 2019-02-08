using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using Kraken.WebSockets.Messages;
using Ping = Kraken.WebSockets.Messages.Ping;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public static class TestSocketMessages
    {
        #region Ping

        public const string PingMessage = @"{""event"":""ping""}";
        
        public static readonly Ping Ping = new Ping();
        
        #endregion
        
        #region SystemStatus
        public const string SystemStatusMessage = 
            "{\"connectionID\":8628615390848610222,\"event\": \"systemStatus\",\"status\": \"online\",\"version\": \"1.0.0\"}";

        public static readonly SystemStatus SystemStatus = new SystemStatus()
        {
            Event = "systemStatus",
            Status = "online",
            ConnectionId = 8628615390848610222,
            Version = "1.0.0"
        };
        #endregion
    }
}