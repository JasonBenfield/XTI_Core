using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class NullStringAsEmptyJsonConverter : JsonConverter<string>
{
    public override bool HandleNull => true;

    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize<string>(ref reader);
        return result ?? "";
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) => 
        JsonSerializer.Serialize(writer, value, new JsonSerializerOptions());
}