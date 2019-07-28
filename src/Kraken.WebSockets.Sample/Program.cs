using System;
using System.Threading.Tasks;
using Kraken.WebSockets.Logging;
using Kraken.WebSockets.Messages;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Kraken.WebSockets.Sample
{
    class Program
    {

        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            new LoggerFactory()
                .EnableLogging()
                .AddSerilog(logger);

            using (var client = KrakenApi.ClientFactory.Create("wss://ws.kraken.com"))
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
            client.HeartbeatReceived += (sender, e) => Console.WriteLine("Heartbeat received");
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

            await client.SubscribeAsync(new Subscribe(new[] { Pair.XBT_EUR }, new SubscribeOptions(SubscribeOptionNames.All)));
        }
    }
}
