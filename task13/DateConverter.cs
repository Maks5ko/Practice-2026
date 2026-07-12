using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public class DateConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateStr = reader.GetString();
            if (!DateTime.TryParseExact(dateStr, Format, null, System.Globalization.DateTimeStyles.None, out var date)) throw new JsonException($"Некорректный формат даты. Ожидается {Format}.");
            return date;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
