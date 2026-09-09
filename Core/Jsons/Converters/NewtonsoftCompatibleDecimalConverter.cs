using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TripleSix.Core.Jsons
{
    /// <summary>
    /// Converter <see cref="decimal"/> tương thích định dạng của Newtonsoft.Json.
    /// Khi ghi, số nguyên luôn kèm phần lẻ (<c>12.0</c> thay cho <c>12</c>, giữ nguyên <c>12.50</c>) để client parse kiểu tĩnh (Dart) không lỗi.
    /// Khi đọc, chấp nhận cả số dạng chuỗi như Newtonsoft.
    /// </summary>
    public class NewtonsoftCompatibleDecimalConverter : JsonConverter<decimal>
    {
        /// <inheritdoc/>
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDecimal();

            if (reader.TokenType == JsonTokenType.String)
            {
                var text = reader.GetString();

                if (string.IsNullOrWhiteSpace(text))
                    throw new JsonException("Expected number or numeric string.");

                return decimal.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);
            }

            throw new JsonException("Expected number or numeric string.");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            var text = value.ToString(CultureInfo.InvariantCulture);

            if (!text.Contains('.'))
                text += ".0";

            writer.WriteRawValue(text);
        }
    }
}
