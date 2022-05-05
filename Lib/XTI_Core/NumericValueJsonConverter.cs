using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class NumericValueJsonConverter<T> : JsonConverter<T>
    where T : NumericValue
{
    private readonly NumericValueJsonConverter converter;

    public NumericValueJsonConverter()
    {
        converter = new NumericValueJsonConverter();
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        (T?)converter.Read(ref reader, typeToConvert, options);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
        converter.Write(writer, value, options);
}

public sealed class NumericValueJsonConverter : JsonConverter<NumericValue>
{
    public override bool HandleNull => true;

    public override bool CanConvert(Type typeToConvert) => typeof(NumericValue).IsAssignableFrom(typeToConvert);

    public override NumericValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        NumericValue numericValue;
        if (reader.TokenType == JsonTokenType.Number)
        {
            var value = reader.GetInt32();
            numericValue = valueFromInt(typeToConvert, value);
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString() ?? "";
            numericValue = (NumericValue)new NumericValueTypeConverter(typeToConvert).ConvertFrom(value)!;
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            var value = 0;
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();
                    if (propName == "Value")
                    {
                        value = reader.GetInt32();
                    }
                }
            }
            numericValue = valueFromInt(typeToConvert, value);
        }
        else
        {
            numericValue = valueFromInt(typeToConvert, 0);
        }
        return numericValue;
    }

    private static NumericValue valueFromInt(Type typeToConvert, int value) =>
        (NumericValue)new NumericValueTypeConverter(typeToConvert).ConvertFrom(value)!;

    public override void Write(Utf8JsonWriter writer, NumericValue value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Value", value.Value);
        writer.WriteString("DisplayText", value.DisplayText);
        writer.WriteEndObject();
    }
}