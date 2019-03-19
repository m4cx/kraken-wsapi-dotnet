using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets.Events
{
    /// <summary>
    /// The information received for a ticker subscription.
    /// </summary>
    public class TickerEventArgs
    {
        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        /// <value>
        /// The channel identifier.
        /// </value>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Gets the pair.
        /// </summary>
        /// <value>
        /// The pair.
        /// </value>
        public string Pair { get; private set; }

        /// <summary>
        /// Gets the ticker.
        /// </summary>
        /// <value>
        /// The ticker.
        /// </value>
        public TickerMessage Ticker { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TickerEventArgs" /> class.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="pair">The pair.</param>
        /// <param name="ticker">The ticker.</param>
        public TickerEventArgs(int channelId, string pair, TickerMessage ticker)
        {
            ChannelId = channelId;
            Pair = pair;
            Ticker = ticker;
        }
    }
}