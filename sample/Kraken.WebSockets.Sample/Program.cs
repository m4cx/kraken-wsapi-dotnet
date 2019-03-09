using System;
using System.Threading.Tasks;
using Kraken.WebSockets.Messages;
using Serilog;

namespace Kraken.WebSockets.Sample
{
    class Program
    {
        private static readonly ILogger logger = Log.ForContext<Program>();

        static KrakenWebSocket kraken;

        static void Main(string[] args)
        {
            // Configure logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Task.Run(RunKraken);
            do
            {
                Console.WriteLine("Press [ESC] to exit.");
            }
            while (Console.ReadKey().Key != ConsoleKey.Escape);

            if (kraken != null)
            {
                Task.Run(async () => await kraken.CloseAsync()).Wait();
            }
        }

        private static async Task RunKraken()
        {
            var uri = "wss://ws-sandbox.kraken.com";
            var serializer = new KrakenMessageSerializer();
            kraken = new KrakenWebSocket(uri, serializer);

            var client = new KrakenApiClient(kraken, serializer);
            client.SystemStatusChanged += (sender, e) => logger.Information("System status changed: {systemStatus}", e.Message);
            await kraken.ConnectAsync();

            kraken.DataReceived += (object sender, Events.KrakenMessageEventArgs e) => 
                logger.Information("Received message: {message}", e.RawContent); ;

            await client.SubscribeAsync(new Subscribe(new[] { "XBT/EUR" }, new SubscribeOptions(SubscribeOptionNames.All)));
        }
    }
}
