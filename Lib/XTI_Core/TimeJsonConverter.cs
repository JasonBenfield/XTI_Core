using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class TimeJsonConverter : JsonConverter<Time>
{
    public override bool HandleNull => true;

    public override Time Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new Time();
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            return Time.Parse(reader.GetString() ?? "");
        }
        throw new NotSupportedException($"Unexpected JSON token '{reader.TokenType}'");
    }

    public override void Write(Utf8JsonWriter writer, Time value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToString());
    }
}