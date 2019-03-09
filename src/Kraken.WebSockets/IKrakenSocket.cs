using System;
using System.Threading.Tasks;
using Kraken.WebSockets.Events;
using Kraken.WebSockets.Messages;

namespace Kraken.WebSockets
{
    /// <summary>
    /// This interface describes a Kraken socket.
    /// </summary>
    public interface IKrakenSocket
    {
        /// <summary>
        /// Occurs when connected.
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        /// Occurs when data received.
        /// </summary>
        event EventHandler<KrakenMessageEventArgs> DataReceived;

        /// <summary>
        /// Connect to the websocket server.
        /// </summary>
        /// <returns>The connect.</returns>
        Task ConnectAsync();

        /// <summary>
        /// Sends the message throught the open websocket.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="message">Message.</param>
        Task SendAsync<TKrakenMessage>(TKrakenMessage message) where TKrakenMessage : IKrakenMessage;

        /// <summary>
        /// Closes the websocket.
        /// </summary>
        /// <returns>The async.</returns>
        Task CloseAsync();
    }
}