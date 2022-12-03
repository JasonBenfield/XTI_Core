using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, object>>
{
    public override Dictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
        }
        return (Dictionary<string, object>)ExtractValue(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
    {
        options = new JsonSerializerOptions(options);
        var converter = options.Converters
            .OfType<DictionaryStringObjectJsonConverter>()
            .FirstOrDefault();
        if (converter != null)
        {
            options.Converters.Remove(converter);
        }
        JsonSerializer.Serialize(writer, value, options);
    }

    private object ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                if (reader.TryGetDateTime(out var date))
                {
                    return date;
                }
                return reader.GetString() ?? "";
            case JsonTokenType.False:
                return false;
            case JsonTokenType.True:
                return true;
            case JsonTokenType.Null:
                return new object();
            case JsonTokenType.Number:
                return reader.GetDecimal();
            case JsonTokenType.StartObject:
                var dictionary = new Dictionary<string, object>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return dictionary;
                    }
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException("JsonTokenType was not PropertyName");
                    }
                    var propertyName = reader.GetString();
                    if (string.IsNullOrWhiteSpace(propertyName))
                    {
                        throw new JsonException("Failed to get property name");
                    }
                    reader.Read();
                    dictionary.Add(propertyName, ExtractValue(ref reader, options));
                }
                return dictionary;
            case JsonTokenType.StartArray:
                var list = new List<object?>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    list.Add(ExtractValue(ref reader, options));
                }
                return list.ToArray();
            default:
                throw new JsonException($"'{reader.TokenType}' is not supported");
        }
    }
}