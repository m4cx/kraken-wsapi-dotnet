using System;
using Kraken.WebSockets.Messages;
using Newtonsoft.Json;

namespace Kraken.WebSockets.Events
{
    /// <summary>
    /// Kraken message event arguments.
    /// </summary>
    public class KrakenMessageEventArgs : EventArgs 
    {
        /// <summary>
        /// Gets the raw content of the message.
        /// </summary>
        /// <value>The content of the raw.</value>
        public string RawContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.Events.KrakenMessageEventArgs"/> class.
        /// </summary>
        /// <param name="rawContent">Raw content.</param>
        public KrakenMessageEventArgs(string rawContent)
        {
            RawContent = rawContent ?? throw new ArgumentNullException(nameof(rawContent));
            if (string.Empty.Equals(rawContent))
            {
                throw new ArgumentOutOfRangeException(nameof(rawContent));
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public TMessage GetMessage<TMessage>() where TMessage : KrakenMessage
        {
            return JsonConvert.DeserializeObject<TMessage>(RawContent);
        }
    }
}
