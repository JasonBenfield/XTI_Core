using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
    public override bool HandleNull => true;

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        TimeSpan ts;
        if (reader.TokenType == JsonTokenType.Number)
        {
            var value = reader.GetInt32();
            ts = TimeSpan.FromMilliseconds(value);
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString() ?? "";
            ts = TimeSpan.Parse(value);
        }
        else
        {
            ts = TimeSpan.MinValue;
        }
        return ts;
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}