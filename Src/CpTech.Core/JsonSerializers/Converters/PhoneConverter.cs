using System;
using CpTech.Core.DataTypes;
using Newtonsoft.Json;

namespace CpTech.Core.JsonSerializers.Converters
{
    public class PhoneConverter : JsonConverter<Phone>
    {
        public override Phone ReadJson(
            JsonReader reader,
            Type objectType,
            Phone existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            return new Phone(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, Phone value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}