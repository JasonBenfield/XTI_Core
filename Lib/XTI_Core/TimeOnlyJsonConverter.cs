using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override bool HandleNull => true;

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new TimeOnly();
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            return TimeOnly.Parse(reader.GetString() ?? "");
        }
        throw new NotSupportedException($"Unexpected JSON token '{reader.TokenType}'");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString("O"));
}