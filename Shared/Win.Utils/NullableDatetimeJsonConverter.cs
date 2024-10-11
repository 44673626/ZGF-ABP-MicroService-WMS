using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Win.Utils
{
    public class NullableDatetimeJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out var date))
                {
                    return date;
                }
            }

            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if(value==null)
                writer.WriteNullValue();
            else
            {
                var valueValue = value.Value;
                if (valueValue.Hour == 0 && valueValue.Minute == 0 && valueValue.Second == 0)
                {
                    writer.WriteStringValue(valueValue.ToString("yyyy-MM-dd"));
                }
                else
                {
                    writer.WriteStringValue(valueValue.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
        }
    }
}