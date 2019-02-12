using System;
using System.Linq;

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
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.Messages.SubscribeOptions"/> class.
        /// </summary>
        /// <param name="name">Name. Valid values are: ticker|ohlc|trade|book|spread|*</param>
        public SubscribeOptions(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (!SubscribeOptionNames.AllowedNames.Any(x => x == name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), name, 
                    $"Allowed values: {string.Join(",", SubscribeOptionNames.AllowedNames)}");
            }
        }
    }
}
