using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kraken.WebSockets.Messages
{
    public class KrakenMessageSerializer : IKrakenMessageSerializer
    {
        private JsonSerializerSettings serializerSettings;

        public KrakenMessageSerializer()
        {
            serializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public TKrakenMessage Deserialize<TKrakenMessage>(string json) where TKrakenMessage : IKrakenMessage
        {
            return JsonConvert.DeserializeObject<TKrakenMessage>(json, serializerSettings);
        }

        public string Serialize<TKrakenMessage>(TKrakenMessage message) where TKrakenMessage : IKrakenMessage
        {
            return JsonConvert.SerializeObject(message, serializerSettings);
        }
    }
}
