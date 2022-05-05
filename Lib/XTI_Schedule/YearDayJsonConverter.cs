using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Schedule;

public sealed class YearDayJsonConverter : JsonConverter<YearDay>
{
    public override YearDay Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new YearDay();
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            return YearDay.Parse(reader.GetString() ?? "");
        }
        throw new NotSupportedException($"Unexpected JSON token '{reader.TokenType}'");
    }

    public override void Write(Utf8JsonWriter writer, YearDay value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Format());
}