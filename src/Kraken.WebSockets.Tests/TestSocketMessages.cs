using System.Diagnostics.CodeAnalysis;
using Kraken.WebSockets.Messages;
using Ping = Kraken.WebSockets.Messages.Ping;

namespace Kraken.WebSockets.Tests
{
    [ExcludeFromCodeCoverage]
    public static class TestSocketMessages
    {
        #region Ping

        public const string PingMessage = @"{""event"":""ping""}";

        public static readonly Ping Ping = new Ping();

        #endregion

        #region SystemStatus

        public const string SystemStatusMessage =
            "{\"connectionID\":8628615390848610222,\"event\": \"systemStatus\",\"status\": \"online\",\"version\": \"1.0.0\"}";

        public static readonly SystemStatus SystemStatus = new SystemStatus
        {
            Event = "systemStatus",
            Status = "online",
            ConnectionId = 8628615390848610222,
            Version = "1.0.0"
        };

        #endregion

        #region SubscriptionStatus1

        public static readonly SubscriptionStatus SubscriptionStatus1 = new SubscriptionStatus
        {
            ChannelId = 123,
            Pair = "XBT/EUR",
            Status = "subscribed"
        };

        public const string SubscriptionStatus1Message =
            @"{""channelID"":123,""event"":""subscriptionStatus"",""pair"":""XBT/EUR"",""status"":""subscribed""}";

        #endregion

        #region SubScriptionStatusNoChannelId

        public static readonly SubscriptionStatus SubScriptionStatusNoChannelId = new SubscriptionStatus
        {
            Pair = "XBT/EUR",
            Status = "subscribed"
        };

        public const string SubScriptionStatusNoChannelIdMessage =
            @"{""event"":""subscriptionStatus"",""pair"":""XBT/EUR"",""status"":""subscribed""}";


        #endregion

        #region UnsubscribeChannelId

        public static readonly Unsubscribe UnsubscribeChannelId = new Unsubscribe(123);

        public const string UnsubscribeChannelIdMessage =
            @"{""channelID"":123,""event"":""unsubscribe""}";

        #endregion

        #region Ticker

        public const string TickerMessage = @"[
  0,
  {
    ""a"": [
      ""5525.40000"",
      1,
      ""1.000""
    ],
    ""b"": [
      ""5525.10000"",
      1,
      ""1.000""
    ],
    ""c"": [
      ""5525.10000"",
      ""0.00398963""
    ],
    ""v"": [
      ""2634.11501494"",
      ""3591.17907851""
    ],
    ""p"": [
      ""5631.44067"",
      ""5653.78939""
    ],
    ""t"": [
      11493,
      16267
    ],
    ""l"": [
      ""5505.00000"",
      ""5505.00000""
    ],
    ""h"": [
      ""5783.00000"",
      ""5783.00000""
    ],
    ""o"": [
      ""5760.70000"",
      ""5763.40000""
    ]
    }
]";

        #endregion
    }
}