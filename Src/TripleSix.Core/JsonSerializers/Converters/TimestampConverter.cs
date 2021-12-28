using System;
using TripleSix.Core.Extensions;
using TripleSix.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripleSix.Core.JsonSerializers.Converters
{
    public class TimestampConverter : DateTimeConverterBase
    {
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            return DateTimeHelper.ParseEpochTimestamp((long)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToEpochTimestamp());
        }
    }
}