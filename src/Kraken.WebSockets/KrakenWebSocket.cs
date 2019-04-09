﻿using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Kraken.WebSockets
{
    /// <summary>
    /// Kraken websocket.
    /// </summary>
    public sealed class KrakenWebSocket : IKrakenSocket
    {
        private static readonly ILogger logger = Log.ForContext<KrakenWebSocket>();
        private static readonly Encoding DEFAULT_ENCODING = Encoding.UTF8;

        private readonly ClientWebSocket webSocket;
        private readonly string uri;
        private readonly IKrakenMessageSerializer serializer;

        /// <summary>
        /// Occurs when connected.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Occurs when data received.
        /// </summary>
        public event EventHandler<KrakenMessageEventArgs> DataReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Kraken.WebSockets.KrakenWebsocket"/> class.
        /// </summary>
        /// <param name="uri">URI.</param>
        public KrakenWebSocket(string uri, IKrakenMessageSerializer serializer)
        {
            this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
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
        public async Task SendAsync<TKrakenMessage>(TKrakenMessage message) where TKrakenMessage : IKrakenMessage
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var jsonMessage = serializer.Serialize<TKrakenMessage>(message);
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

            logger.Warning("WebSocket is not open. Current state: {state}",
                webSocket.State);
        }

        /// <summary>
        /// Closes the websocket.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task CloseAsync()
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Connection closed by consumer",
                    CancellationToken.None);
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
                    logger.Debug("Waiting for new message");
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            logger.Debug("Closing connection to socket");
                            await
                                webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                            logger.Debug("Connection successfully closed");
                            // TODO: Disconnected-Event
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

                    string eventString = null;
                    int? channelId = null;

                    if (!string.IsNullOrEmpty(message))
                    {
                        var token = JToken.Parse(message);
                        if (token is JObject)
                        {
                            var messageObj = JObject.Parse(message);
                            eventString = (string)messageObj.GetValue("event");
                        }
                        else if (token is JArray)
                        {
                            var arrayToken = token as JArray;
                            channelId = (int)arrayToken.First;
                            eventString = "data";
                    
                        }
                    }

                    InvokeDataReceived(new KrakenMessageEventArgs(eventString, message, channelId));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while listenening or reading new messages from WebSocket");
                // TODO: Disconnected-Event
                throw;
            }
            finally
            {
                logger.Information("Closing WebSocket");
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
