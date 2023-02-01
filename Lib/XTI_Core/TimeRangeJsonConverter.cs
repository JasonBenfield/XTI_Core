using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class TimeRangeJsonConverter : JsonConverter<TimeRange>
{
    public override bool HandleNull => true;

    public override TimeRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var startTime = new TimeOnly();
        var duration = TimeSpan.MaxValue;
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new TimeRange();
        }
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();
                    if (propName == nameof(TimeRange.Start))
                    {
                        var timeOptions = new JsonSerializerOptions();
                        timeOptions.Converters.Add(new TimeOnlyJsonConverter());
                        startTime = JsonSerializer.Deserialize<TimeOnly>(ref reader, timeOptions);
                    }
                    else if (propName == nameof(TimeRange.Duration))
                    {
                        var tsOptions = new JsonSerializerOptions();
                        tsOptions.Converters.Add(new TimeSpanJsonConverter());
                        duration = JsonSerializer.Deserialize<TimeSpan>(ref reader, tsOptions);
                    }
                }
            }
        }
        return new TimeRange(startTime, duration);
    }

    public override void Write(Utf8JsonWriter writer, TimeRange value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(nameof(TimeRange.Start), value.Start.ToString("O"));
        writer.WriteString(nameof(TimeRange.Duration), value.Duration.ToString());
        writer.WriteEndObject();
    }
}