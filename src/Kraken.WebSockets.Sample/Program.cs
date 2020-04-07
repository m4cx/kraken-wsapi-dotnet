using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Kraken.WebSockets.Authentication;
using Kraken.WebSockets.Logging;
using Kraken.WebSockets.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Kraken.WebSockets.Sample
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        private static AuthToken token;

        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RollingFile("logs/kraken-websockets.log")
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            new LoggerFactory()
                .AddKrakenWebSockets()
                .AddSerilog(logger);

            var krakenApi = new KrakenApi()
                .ConfigureAuthentication(
                    configuration.GetValue<string>("API_URL"),
                    configuration.GetValue<string>("API_KEY"),
                     configuration.GetValue<string>("API_SECRET"))
                .ConfigureWebsocket(configuration.GetValue<string>("WS_ENDPOINT"));

            token = await krakenApi.AuthenticationClient.GetWebsocketToken();

            using (var client = krakenApi.BuildClient())
            {
                await Task.Run(() => RunKraken(client, token));
                do
                {
                    Console.WriteLine("Press [ESC] to exit.");
                }
                while (Console.ReadKey().Key != ConsoleKey.Escape);
            }
        }

        private static async Task RunKraken(IKrakenApiClient client, AuthToken token)
        {
            client.HeartbeatReceived += (sender, e) => Console.WriteLine("Heartbeat received");
            client.SystemStatusChanged += (sender, e) => Console.WriteLine($"System status changed: status={e.Message.Status}");
            client.SubscriptionStatusChanged += (sender, e) => Console.WriteLine($"Subscription status changed: status={e.Message.Status}, pair={e.Message.Pair}, channelId={e.Message.ChannelId}, error={e.Message.ErrorMessage}, subscription.name={e.Message.Subscription.Name}");
            client.TickerReceived += (sender, e) => Console.WriteLine($"Ticker received");
            client.OhlcReceived += (sender, e) => Console.WriteLine($"Ohlc received");
            client.TradeReceived += (sender, e) => Console.WriteLine($"Trade received");
            client.SpreadReceived += (sender, e) => Console.WriteLine($"Spread received");
            client.BookSnapshotReceived += (sender, e) => Console.WriteLine($"BookSnapshot received");
            client.BookUpdateReceived += (sender, e) => Console.WriteLine($"BookUpdate received");
            client.OwnTradesReceived += (sender, e) => Console.WriteLine($"OwnTrades received");
            client.OpenOrdersReceived += (sender, e) => Console.WriteLine($"OpenOrders received");

            await client.ConnectAsync();

            client.SubscriptionStatusChanged += async (sender, e) =>
            {
                if (e.Message.Status == SubscriptionStatusNames.Subscribe && e.Message.ChannelId.HasValue)
                {
                    await Task.Delay(50000);
                    await client.UnsubscribeAsync(e.Message.ChannelId.Value);
                }
            };

            await client.SubscribeAsync(new Subscribe(new[] { Pair.XBT_EUR }, new SubscribeOptions(SubscribeOptionNames.All)));
            await client.SubscribeAsync(new Subscribe(null, new SubscribeOptions(SubscribeOptionNames.OwnTrades, token.Token)));
            await client.SubscribeAsync(new Subscribe(null, new SubscribeOptions(SubscribeOptionNames.OpenOrders, token.Token)));

            await client.AddOrder(new AddOrderCommand(token.Token, OrderType.Market, Side.Sell, "XBT/USD", 1)
            {
                TradingAgreement = "agree"
            });
        }
    }
}
