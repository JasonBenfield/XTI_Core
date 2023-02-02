using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class DateRangeJsonConverter : JsonConverter<DateRange>
{
    public override bool HandleNull => true;

    public override DateRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var start = DateOnly.MinValue;
        var end = DateOnly.MaxValue;
        if (reader.TokenType == JsonTokenType.Null)
        {
            return DateRange.All();
        }
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var dateOptions = new JsonSerializerOptions();
            dateOptions.Converters.Add(new DateOnlyJsonConverter());
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();
                    if (propName == nameof(DateRange.Start))
                    {
                        start = JsonSerializer.Deserialize<DateOnly>(ref reader, dateOptions);
                    }
                    else if (propName == nameof(DateRange.End))
                    {
                        end = JsonSerializer.Deserialize<DateOnly>(ref reader, dateOptions);
                    }
                }
            }
        }
        return DateRange.Between(start, end);
    }

    public override void Write(Utf8JsonWriter writer, DateRange value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(DateRange.Start), value.Start.ToString("O"));
        writer.WriteString(nameof(DateRange.End), value.End.ToString("O"));
        writer.WriteEndObject();
    }
}