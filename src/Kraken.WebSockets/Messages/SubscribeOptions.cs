using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Kraken.WebSockets.Messages
{
    /// <summary>
    /// Subscribe options.
    /// </summary>
    public class SubscribeOptions
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public int? Interval { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int? Depth { get; set; }

        /// <summary>
        /// Gets the authentication token for private subscriptions.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.Messages.SubscribeOptions" /> class.
        /// </summary>
        /// <param name="name">Name. Valid values are: ticker|ohlc|trade|book|spread|ownTrades|*</param>
        /// <param name="token">The authentication token for private subscriptions.</param>
        /// <exception cref="ArgumentNullException">name</exception>
        /// <exception cref="ArgumentOutOfRangeException">name - Allowed values: ticker|ohlc|trade|book|spread|ownTrades|*</exception>
        public SubscribeOptions(string name, string token = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (SubscribeOptionNames.AllowedNames.All(x => x != name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), name, 
                    $"Allowed values: {string.Join(",", SubscribeOptionNames.AllowedNames)}");
            }

            Token = token;
            if (IsPrivate(name) && string.IsNullOrEmpty(Token))
            {
                throw new ArgumentNullException(nameof(token), $"An authentication token has to be provided for private subscriptions.");
            }

        }

        private bool IsPrivate(string name)
        {
            return SubscribeOptionNames.PrivateNames.Contains(name);
        }
    }
}
