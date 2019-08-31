using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.WebSockets.Events
{
    public sealed class KrakenPrivateEventArgs<TPrivate> : EventArgs
    {
        public TPrivate PrivateMessage { get; }

        public KrakenPrivateEventArgs(TPrivate privateMessage)
        {
            PrivateMessage = privateMessage;
        }
    }
}
