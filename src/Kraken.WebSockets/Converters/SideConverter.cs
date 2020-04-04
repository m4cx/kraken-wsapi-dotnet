using Newtonsoft.Json;
using System;

namespace Kraken.WebSockets.Converters
{
    internal sealed class SideConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Side);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return (((string)reader.Value).ToLower()) switch
            {
                "buy" => Side.Buy,
                "sell" => Side.Sell,
                _ => throw new ArgumentOutOfRangeException(nameof(reader.Value)),
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => writer.WriteValue(Enum.GetName(typeof(Side), value).ToLower());
    }
}
