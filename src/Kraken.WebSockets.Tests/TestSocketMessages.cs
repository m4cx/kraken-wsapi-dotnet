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

        public const string TickerMessage = @"[0,{""a"": [""5525.40000"",1,""1.000""],""b"": [""5525.10000"",1,""1.000""],""c"": [""5525.10000"",""0.00398963""],""v"": [""2634.11501494"",""3591.17907851""],""p"": [""5631.44067"",""5653.78939""],""t"": [11493,16267],""l"": [""5505.00000"",""5505.00000""],""h"": [""5783.00000"",""5783.00000""],""o"": [""5760.70000"",""5763.40000""]}]";

        #endregion

        #region Ohlc

        public const string OhlcMessage = @"[42,[""1542057314.748456"",""1542057360.435743"",""3586.70000"",""3586.70000"",""3586.60000"",""3586.60000"",""3586.68894"",""0.03373000"",2]]";

        #endregion

        #region Trade

        public const string TradeMessage = @"[0,[[""5541.20000"",""0.15850568"",""1534614057.321597"",""s"",""l"",""""],[""6060.00000"",""0.02455000"",""1534614057.324998"",""b"",""l"",""""]]]";

        #endregion

        #region Spread

        public const string SpreadMessage = @"[0,[""5698.40000"",""5700.00000"",""1542057299.545897""]]";

        #endregion

        #region BookSnapshot

        public const string BookSnapshotMessage = @"[0,{""as"":[[""5541.30000"",""2.50700000"",""1534614248.123678""],
      [
          ""5541.80000"",
          ""0.33000000"",
          ""1534614098.345543""
      ],
      [
          ""5542.70000"",
          ""0.64700000"",
          ""1534614244.654432""
      ]
    ],
    ""bs"": [
      [
          ""5541.20000"",
          ""1.52900000"",
          ""1534614248.765567""
      ],
      [
          ""5539.90000"",
          ""0.30000000"",
          ""1534614241.769870""
      ],
      [
          ""5539.50000"",
          ""5.00000000"",
          ""1534613831.243486""
      ]
    ]
  }
]";

        #endregion

        #region BookUpdate

        public const string BookUpdateCompleteMessage = @"[
  1234,
  {""a"": [
    [
      ""5541.30000"",
      ""2.50700000"",
      ""1534614248.456738""
    ],
    [
      ""5542.50000"",
      ""0.40100000"",
      ""1534614248.456738""
    ]
  ]
  },
  {""b"": [
    [
      ""5541.30000"",
      ""0.00000000"",
      ""1534614335.345903""
    ]
  ]
  }
]";

        #endregion
    }
}