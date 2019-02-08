using Newtonsoft.Json;

namespace Kraken.WebSockets.Messages
{
    public class SystemStatus : IKrakenMessage
    {
        [JsonProperty("connectionID")]
        public decimal ConnectionId { get; private set; }

        [JsonProperty("event")]
        public string Event { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("version")]
        public string Version { get; private set; }
    }
}