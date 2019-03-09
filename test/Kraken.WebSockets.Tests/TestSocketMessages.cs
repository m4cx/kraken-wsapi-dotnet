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
    }
}