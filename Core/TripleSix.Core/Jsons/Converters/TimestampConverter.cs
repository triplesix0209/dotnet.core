using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Jsons
{
    public class TimestampConverter : DateTimeConverterBase
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            return ((long)reader.Value).ToDateTime();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(value == null ? null : ((DateTime)value).ToEpochTimestamp());
        }
    }
}
