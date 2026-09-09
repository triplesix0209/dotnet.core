using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TripleSix.Core.Jsons
{
    /// <summary>
    /// Converter <see cref="double"/> tương thích định dạng của Newtonsoft.Json.
    /// Khi ghi, số thực nguyên luôn kèm phần lẻ (<c>1.0</c> thay cho <c>1</c>) để client parse kiểu tĩnh (Dart) không lỗi;
    /// <c>NaN</c>/<c>Infinity</c> được ghi dạng chuỗi. Khi đọc, chấp nhận cả số dạng chuỗi như Newtonsoft.
    /// </summary>
    public class NewtonsoftCompatibleDoubleConverter : JsonConverter<double>
    {
        /// <inheritdoc/>
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDouble();

            if (reader.TokenType == JsonTokenType.String)
            {
                var text = reader.GetString();

                if (string.IsNullOrWhiteSpace(text))
                    throw new JsonException("Expected number or numeric string.");

                return double.Parse(text, NumberStyles.Float, CultureInfo.InvariantCulture);
            }

            throw new JsonException("Expected number or numeric string.");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
                return;
            }

            var text = value.ToString("R", CultureInfo.InvariantCulture);

            if (text.IndexOfAny(['.', 'E', 'e']) < 0)
                text += ".0";

            writer.WriteRawValue(text);
        }
    }
}
