using System;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Extensions;
using Kraken.WebSockets.Messages;
using Serilog;

namespace Kraken.WebSockets
{
    /// <summary>
    /// Kraken API client.
    /// </summary>
    public sealed class KrakenApiClient : IKrakenApiClient
    {
        private static readonly ILogger logger = Log.ForContext<KrakenApiClient>();

        private readonly IKrakenSocket socket;
        private readonly IKrakenMessageSerializer serializer;

        /// <summary>
        /// Gets the system status.
        /// </summary>
        /// <value>The system status.</value>
        public SystemStatus SystemStatus { get; private set; }

        /// <summary>
        /// Occurs when system status changed.
        /// </summary>
        public event EventHandler<KrakenMessageEventArgs<SystemStatus>> SystemStatusChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.KrakenApiClient"/> class.
        /// </summary>
        /// <param name="socket">Socket.</param>
        public KrakenApiClient(IKrakenSocket socket, IKrakenMessageSerializer serializer)
        {
            this.socket = socket ?? throw new ArgumentNullException(nameof(socket));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            // Add watch for incoming messages 
            this.socket.DataReceived += HandleIncomingMessage;
        }

        #region Private Helper

        private void HandleIncomingMessage(object sender, KrakenMessageEventArgs eventArgs)
        {
            logger.Debug("Handling incomming message");

            // handle 'systemStatus' event
            if (eventArgs.Event == "systemStatus")
            {
                var systemStatus = serializer.Deserialize<SystemStatus>(eventArgs.RawContent);
                logger.Verbose("System status changed: {@systemStatus}", systemStatus);
                SystemStatus = systemStatus;
                SystemStatusChanged.InvokeAll(this, systemStatus);
            }
        }

        #endregion
    }
}
