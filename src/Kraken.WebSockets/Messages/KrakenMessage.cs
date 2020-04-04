namespace Kraken.WebSockets.Messages
{
    /// <summary>
    /// Kraken message.
    /// </summary>
    public class KrakenMessage : IKrakenMessage
    {
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <value>The event.</value>
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
