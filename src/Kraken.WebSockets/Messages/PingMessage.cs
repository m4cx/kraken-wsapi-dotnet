using Newtonsoft.Json;

namespace Kraken.WebSockets.Messages
{
    /// <summary>
    /// Ping message.
    /// </summary>
    public class PingMessage : KrakenMessage
    {
        private const string EVENT_TYPE = "ping";

        /// <summary>
        /// Gets the request identifier.
        /// </summary>
        /// <value>The request identifier.</value>
        [JsonProperty("reqid", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public int? RequestId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.Messages.PingMessage"/> class.
        /// </summary>
        /// <param name="requestId">Request identifier.</param>
        public PingMessage(int? requestId = null) : base(EVENT_TYPE)
        {
            RequestId = requestId;
        }
    }
}
