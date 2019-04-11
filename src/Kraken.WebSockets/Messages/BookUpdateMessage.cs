using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.WebSockets.Messages
{
    /// <summary>
    /// Order book levels. Following the snapshot, level updates will be published.
    /// </summary>
    public sealed class BookUpdateMessage
    {
        /// <summary>
        /// Gets the ChannelID of pair-order book levels subscription.
        /// </summary>
        /// <value>
        /// The ChannelID of pair-order book levels subscription.
        /// </value>
        public long ChannelId { get; private set; }

        /// <summary>
        /// Gets the Ask array of level updates..
        /// </summary>
        /// <value>
        /// The Ask array of level updates..
        /// </value>
        public PriceLevel[] Asks { get; private set; }

        /// <summary>
        /// Gets the Bid array of level updates..
        /// </summary>
        /// <value>
        /// The Bid array of level updates..
        /// </value>
        public PriceLevel[] Bids { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="BookUpdateMessage"/> class from being created.
        /// </summary>
        private BookUpdateMessage()
        {
        }

        /// <summary>
        /// Creates from string.
        /// </summary>
        /// <param name="rawBookUpdateMessage">The raw book update message.</param>
        /// <returns></returns>
        public static BookUpdateMessage CreateFromString(string rawBookUpdateMessage)
        {
            var bookUpdateMessage = KrakenDataMessageHelper.EnsureRawMessage(rawBookUpdateMessage);

            var asks = bookUpdateMessage.Skip(1).OfType<JObject>().Where(x => x.ContainsKey("a")).FirstOrDefault();
            var bids = bookUpdateMessage.Skip(1).OfType<JObject>().Where(x => x.ContainsKey("b")).FirstOrDefault();

            return new BookUpdateMessage
            {
                ChannelId = Convert.ToInt64(bookUpdateMessage.First),
                Asks = asks!= null ? asks["a"].OfType<JArray>().Select(level => PriceLevel.CreateFromJArray(level)).ToArray() : null,
                Bids = bids != null ? bids["b"].OfType<JArray>().Select(level => PriceLevel.CreateFromJArray(level)).ToArray() : null,
            };
        }
    }
}
