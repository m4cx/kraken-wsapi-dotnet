using System.Collections.Generic;

namespace Kraken.WebSockets.Messages
{
    /// <summary>
    /// Subscribe option names.
    /// </summary>
    /// <remarks>
    /// public: ticker|ohlc|trade|book|spread|*
    /// private: ownTrades (you need to provide an authentication token)
    /// </remarks>
    public static class SubscribeOptionNames
    {
        /// <summary>
        /// All information.
        /// </summary>
        public const string All = "*";

        /// <summary>
        /// Ticker information includes best ask and best bid prices, 
        /// 24hr volume, last trade price, volume weighted average price, 
        /// etc for a given currency pair. A ticker message is published 
        /// every time a trade or a group of trade happens.
        /// </summary>
        public const string Ticker = "ticker";

        /// <summary>
        /// Open High Low Close (Candle) feed for a currency pair 
        /// and interval period.
        /// </summary>
        public const string OHLC = "ohlc";

        /// <summary>
        /// Open High Low Close (Candle) feed for a currency pair and interval period.
        /// </summary>
        public const string Trade = "trade";

        /// <summary>
        /// Order book levels. On subscription, a snapshot will be published at 
        /// the specified depth, following the snapshot, level updates will be 
        /// published.
        /// </summary>
        public const string Book = "book";

        /// <summary>
        /// Spread feed to show best bid and ask price for a currency pair
        /// </summary>
        public const string Spread = "spread";

        /// <summary>
        /// Own trades, on subscription last 50 trades for the user will be sent, followed by new trades.
        /// </summary>
        public const string OwnTrades = "ownTrades";

        internal static IEnumerable<string> AllowedNames
        {
            get
            {
                yield return All;

                yield return Ticker;
                yield return OHLC;
                yield return Trade;
                yield return Book;
                yield return Spread;
                yield return OwnTrades;
            }
        }

        internal static IEnumerable<string> PrivateNames
        {
            get { yield return OwnTrades; }
        }
    }
}
