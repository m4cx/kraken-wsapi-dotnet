using System;
using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets.Events
{
    /// <summary>
    /// Kraken message event arguments.
    /// </summary>
    public class KrakenMessageEventArgs : EventArgs
    {
        private readonly IKrakenMessageSerializer serializer;

        /// <summary>
        /// Gets the raw content of the message.
        /// </summary>
        /// <value>The content of the raw.</value>
        public string RawContent { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>The message.</returns>
        /// <typeparam name="TKrakenMessage">The 1st type parameter.</typeparam>
        public TKrakenMessage GetMessage<TKrakenMessage>() where TKrakenMessage : IKrakenMessage
        {
            return serializer.Deserialize<TKrakenMessage>(RawContent);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.Events.KrakenMessageEventArgs"/> class.
        /// </summary>
        /// <param name="rawContent">Raw content.</param>
        public KrakenMessageEventArgs(string rawContent, IKrakenMessageSerializer serializer)
        {
            RawContent = rawContent ?? throw new ArgumentNullException(nameof(rawContent));
            this.serializer = serializer;

            if (string.Empty.Equals(rawContent))
            {
                throw new ArgumentOutOfRangeException(nameof(rawContent));
            }
        }
    }
}
