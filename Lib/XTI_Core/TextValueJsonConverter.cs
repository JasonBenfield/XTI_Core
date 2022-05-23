using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class TextValueJsonConverter<T> : JsonConverter<T>
    where T : TextValue
{
    public override bool HandleNull => true;

    public override bool CanConvert(Type typeToConvert) => true;

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        T textValue;
        if (reader.TokenType == JsonTokenType.Null || reader.TokenType == JsonTokenType.String)
        {
            var value = reader.TokenType == JsonTokenType.Null
                ? ""
                : reader.GetString() ?? "";
            var ctor = typeToConvert.GetConstructor(new[] { typeof(string) });
            if (ctor == null)
            {
                ctor = typeToConvert.GetConstructor(new[] { typeof(string), typeof(string) });
                if (ctor == null)
                {
                    throw new ArgumentNullException("ctor", $"ctor not found for {typeToConvert.Name}");
                }
                textValue = (T)(ctor.Invoke(new[] { value, value }) ?? throw new ArgumentNullException("ctor", $"ctor for {typeToConvert.Name} returned null"));
            }
            else
            {
                textValue = (T)(ctor.Invoke(new[] { value }) ?? throw new ArgumentNullException("ctor", $"ctor for {typeToConvert.Name} returned null"));
            }
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            var value = "";
            var displayText = "";
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();
                    if (propName == "Value")
                    {
                        value = reader.GetString();
                    }
                    if (propName == "DisplayText")
                    {
                        displayText = reader.GetString();
                    }
                }
            }
            var ctor = typeToConvert.GetConstructor(new[] { typeof(string), typeof(string) });
            if (ctor == null)
            {
                ctor = typeToConvert.GetConstructor(new[] { typeof(string) });
                if (ctor == null)
                {
                    throw new ArgumentNullException("ctor", $"ctor not found for {typeToConvert.Name}");
                }
                textValue = (T)(ctor.Invoke(new[] { displayText }) ?? throw new ArgumentNullException($"ctor for {typeToConvert.Name} return null"));
            }
            else
            {
                textValue = (T)(ctor.Invoke(new[] { value, displayText }) ?? throw new ArgumentNullException($"ctor for {typeToConvert.Name} return null"));
            }
        }
        else
        {
            throw new NotSupportedException("Expected StartObject token");
        }
        return textValue;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Value", value.Value);
        writer.WriteString("DisplayText", value.DisplayText);
        writer.WriteEndObject();
    }
}
