using System;
namespace Kraken.WebSockets.Messages
{
    public interface IKrakenMessageSerializer
    {
        TKrakenMessage Deserialize<TKrakenMessage>(string json) where TKrakenMessage : IKrakenMessage;

        string Serialize<TKrakenMessage>(TKrakenMessage message) where TKrakenMessage : IKrakenMessage; 
    }
}
