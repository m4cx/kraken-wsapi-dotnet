using System;
using System.Threading.Tasks;
using Kraken.WebSockets.Messages;
using Serilog;

namespace Kraken.WebSockets.Sample
{
    class Program
    {
        private static readonly ILogger logger = Log.ForContext<Program>();

        static void Main(string[] args)
        {
            // Configure logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            var uri = "wss://ws-beta.kraken.com";
            using (var client = KrakenApi.ClientFactory.Create(uri))
            {
                Task.Run(() => RunKraken(client));
                do
                {
                    Console.WriteLine("Press [ESC] to exit.");
                }
                while (Console.ReadKey().Key != ConsoleKey.Escape);
            }
        }

        private static async Task RunKraken(IKrakenApiClient client)
        {
            client.SystemStatusChanged += (sender, e) => Console.WriteLine($"System status changed: status={e.Message.Status}");
            client.SubscriptionStatusChanged += (sender, e) => Console.WriteLine($"Subscription status changed: status={e.Message.Status}, pair={e.Message.Pair}, channelId={e.Message.ChannelId}, error={e.Message.ErrorMessage}, subscription.name={e.Message.Subscription.Name}"); ;
            client.TickerReceived += (sender, e) => Console.WriteLine($"Ticker received");
            client.OhlcReceived += (sender, e) => Console.WriteLine($"Ohlc received");
            client.TradeReceived += (sender, e) => Console.WriteLine($"Trade received");
            client.SpreadReceived += (sender, e) => Console.WriteLine($"Spread received");
            client.BookSnapshotReceived += (sender, e) => Console.WriteLine($"BookSnapshot received");
            client.BookUpdateReceived += (sender, e) => Console.WriteLine($"BookUpdate received");

            await client.ConnectAsync();

            client.SubscriptionStatusChanged += async (sender, e) =>
            {
                if (e.Message.Status == SubscriptionStatusNames.Subscribe && e.Message.ChannelId.HasValue)
                {
                    await Task.Delay(50000);
                    await client.UnsubscribeAsync(e.Message.ChannelId.Value);
                }
            };

            await client.SubscribeAsync(new Subscribe(new[] { "XBT/EUR" }, new SubscribeOptions(SubscribeOptionNames.All)));
        }
    }
}
