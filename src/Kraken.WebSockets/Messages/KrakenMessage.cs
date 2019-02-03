using Newtonsoft.Json;

namespace Kraken.WebSockets.Messages
{
    /// <summary>
    /// Kraken message.
    /// </summary>
    public abstract class KrakenMessage
    {
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <value>The event.</value>
        [JsonProperty(Order = 0)]
        public string Event { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.KrakenMessage"/> class.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        protected KrakenMessage(string eventType)
        {
            Event = eventType;
        }
    }
}
