using System.Text.Json;
using System.Text.Json.Serialization;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Jsons
{
    /// <summary>
    /// Timestamp converter.
    /// </summary>
    public class TimestampConverter : JsonConverter<DateTime>
    {
        /// <inheritdoc/>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt64().ToDateTime();
            }
            if (reader.TokenType == JsonTokenType.String && long.TryParse(reader.GetString(), out var timestamp))
            {
                return timestamp.ToDateTime();
            }
            throw new JsonException("Expected number or string representing unix timestamp.");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToEpochTimestamp());
        }
    }
}
