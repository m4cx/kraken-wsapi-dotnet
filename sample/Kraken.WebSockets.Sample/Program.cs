using System;
using System.Threading.Tasks;
using Kraken.WebSockets.Messages;
using Serilog;

namespace Kraken.WebSockets.Sample
{
    class Program
    {
        private static readonly ILogger logger = Log.ForContext<Program>();

        static object consoleLock = new object();
        static KrakenWebSocket kraken;

        static void Main(string[] args)
        {
            // Configure logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
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
            kraken = new KrakenWebSocket(uri);
            await kraken.ConnectAsync();

            kraken.DataReceived += (object sender, Events.KrakenMessageEventArgs e) => 
                logger.Information("Received message: {message}", e.RawContent); ;

            int requestId = 1;
            while (true)
            {
                await kraken.SendAsync(new PingMessage(requestId++));
                await Task.Delay(5000);
            }
        }
    }
}
