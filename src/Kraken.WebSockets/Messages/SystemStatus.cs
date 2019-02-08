using Newtonsoft.Json;

namespace Kraken.WebSockets.Messages
{
    public class SystemStatus : IKrakenMessage
    {
        [JsonProperty("connectionID")]
        public decimal ConnectionId { get; internal set; }

        [JsonProperty("event")]
        public string Event { get; internal set; }

        [JsonProperty("status")]
        public string Status { get; internal set; }

        [JsonProperty("version")]
        public string Version { get; internal set; }
    }
}