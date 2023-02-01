using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override bool HandleNull => true;

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new DateOnly();
        }
        if (reader.TokenType == JsonTokenType.String)
        {
            return DateOnly.Parse(reader.GetString() ?? "");
        }
        throw new NotSupportedException($"Unexpected JSON token '{reader.TokenType}'");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString("O"));
}