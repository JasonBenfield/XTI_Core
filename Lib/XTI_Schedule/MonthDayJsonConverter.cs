using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Schedule;

public sealed class MonthDayJsonConverter : JsonConverter<MonthDay>
{
    public override MonthDay Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new MonthDay();
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            return MonthDay.Parse(reader.GetString() ?? "");
        }
        if (reader.TokenType == JsonTokenType.Number)
        {
            return new MonthDay(reader.GetInt32());
        }
        throw new NotSupportedException($"Unexpected JSON token '{reader.TokenType}'");
    }

    public override void Write(Utf8JsonWriter writer, MonthDay value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Format());
    }
}