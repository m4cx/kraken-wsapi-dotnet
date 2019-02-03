﻿using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Kraken.WebSockets
{
    /// <summary>
    /// Kraken websocket.
    /// </summary>
    public sealed class KrakenWebSocket
    {
        private static readonly ILogger logger = Log.ForContext<KrakenWebSocket>();
        private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;

        private ClientWebSocket webSocket;
        private readonly string uri;

        public event EventHandler Connected;
        public event EventHandler<KrakenMessageEventArgs> DataReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.KrakenWebsocket"/> class.
        /// </summary>
        /// <param name="uri">URI.</param>
        public KrakenWebSocket(string uri)
        {
            this.uri = uri;
            webSocket = new ClientWebSocket();
        }

        /// <summary>
        /// Connect to the websocket server.
        /// </summary>
        /// <returns>The connect.</returns>
        public async Task ConnectAsync()
        {
            try
            {
                logger.Information("Trying to connect to '{uri}'", uri);
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                InvokeConnected();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                StartListening();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while connecting to '{uri}'", uri);
                throw;
            }
        }

        /// <summary>
        /// Sends the message throught the open websocket.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        public async Task SendAsync(KrakenMessage message)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                logger.Verbose("Trying to send: {message}", jsonMessage);

                var messageBytes = DEFAULT_ENCODING.GetBytes(jsonMessage);
                await webSocket.SendAsync(
                    new ArraySegment<byte>(messageBytes), 
                    WebSocketMessageType.Text, 
                    true, 
                    CancellationToken.None);
                logger.Verbose("Successfully sent: {message}", jsonMessage);
                return;
            }

            logger.Warning("WebSocket is not open. Current state: {state}", webSocket.State);
        }

        /// <summary>
        /// Closes the websocket.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task CloseAsync()
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by consumer", CancellationToken.None);
            }
        }

        #region Private Helper

        private async Task StartListening()
        {
            var buffer = new byte[1024];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var messageParts = new StringBuilder();
                    logger.Information("Waiting for new message");
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            // Disconnected-Event
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            messageParts.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    var message = messageParts.ToString();
                    logger.Debug("Received new message from websocket");
                    logger.Verbose("Received: {message}", message);

                    InvokeDataReceived(new KrakenMessageEventArgs(message));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while listenening or reading new messages from WebSocket");
                // Disconnected-Event
                throw;
            }
            finally
            {
                webSocket.Dispose();
            }
        }

        private void InvokeConnected()
        {
            var connectedHandler = Connected;
            if (connectedHandler == null) return;

            InvokeAllHandlers(connectedHandler.GetInvocationList(), new EventArgs());
        }

        private void InvokeDataReceived(KrakenMessageEventArgs krakenMessageEventArgs)
        {
            var dataReceivedHandler = DataReceived;
            if (dataReceivedHandler == null) return;

            InvokeAllHandlers(dataReceivedHandler.GetInvocationList(), krakenMessageEventArgs);
        }

        private void InvokeAllHandlers(Delegate[] handlers, EventArgs eventArgs)
        {
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    handler.DynamicInvoke(this, eventArgs);
                }
            }
        }

        #endregion
    }
}
