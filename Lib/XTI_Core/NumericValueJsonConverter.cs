using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class NumericValueJsonConverter<T> : JsonConverter<T>
    where T : NumericValue
{
    public override bool HandleNull => true;

    public override bool CanConvert(Type typeToConvert) => typeof(NumericValue).IsAssignableFrom(typeToConvert);

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        T numericValue;
        if (reader.TokenType == JsonTokenType.Null)
        {
            var valuesField = typeToConvert.GetField("Values", BindingFlags.Static | BindingFlags.Public);
            if (valuesField == null)
            {
                throw new ArgumentNullException("Values field not found");
            }
            var values = valuesField.GetValue(typeToConvert);
            var getDefaultMethod = valuesField.FieldType.GetMethod("GetDefault", new Type[0]);
            if (getDefaultMethod == null)
            {
                throw new ArgumentNullException("Value method not found");
            }
            var defaultValue = (T?)getDefaultMethod.Invoke(values, new object[0]);
            numericValue = defaultValue ?? throw new ArgumentNullException("numeric value should not be null");
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            var value = reader.GetInt32();
            numericValue = valueFromInt(value);
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString() ?? "";
            numericValue = (T)new NumericValueTypeConverter<T>().ConvertFrom(value)!;
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
            numericValue = valueFromInt(value);
        }
        else
        {
            numericValue = valueFromInt(0);
        }
        return numericValue;
    }

    private static T valueFromInt(int value) => (T)new NumericValueTypeConverter<T>().ConvertFrom(value)!;

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Value", value.Value);
        writer.WriteString("DisplayText", value.DisplayText);
        writer.WriteEndObject();
    }
}