using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class NumericValueJsonConverter : JsonConverter<NumericValue>
{
    public override bool CanConvert(Type typeToConvert)
        => typeof(NumericValue).IsAssignableFrom(typeToConvert);

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
            numericValue = valueFromString(typeToConvert, value);
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

    private static NumericValue valueFromInt(Type typeToConvert, int value)
    {
        var valuesField = typeToConvert.GetField("Values", BindingFlags.Static | BindingFlags.Public);
        if (valuesField == null)
        {
            throw new ArgumentNullException("Values field not found");
        }
        var values = valuesField.GetValue(typeToConvert);
        var valueMethod = valuesField.FieldType.GetMethod("Value", new[] { typeof(int) });
        if(valueMethod == null)
        {
            throw new ArgumentNullException("Value method not found");
        }
        var numericValue = valueMethod.Invoke(values, new[] { (object)value });
        if (numericValue == null)
        {
            throw new ArgumentNullException("numeric value should not be null");
        }
        return (NumericValue)numericValue;
    }

    private static NumericValue valueFromString(Type typeToConvert, string value)
    {
        var valuesField = typeToConvert.GetField("Values", BindingFlags.Static | BindingFlags.Public);
        if (valuesField == null)
        {
            throw new ArgumentNullException("Values field not found");
        }
        var values = valuesField.GetValue(typeToConvert);
        var valueMethod = valuesField.FieldType.GetMethod("Value", new[] { typeof(string) }); 
        if (valueMethod == null)
        {
            throw new ArgumentNullException("Value method not found");
        }
        var numericValue = valueMethod.Invoke(values, new[] { value });
        if(numericValue == null)
        {
            throw new ArgumentNullException("numeric value should not be null");
        }
        return (NumericValue)numericValue;
    }

    public override void Write(Utf8JsonWriter writer, NumericValue value, JsonSerializerOptions options)
    {
        var writeOptions = new JsonSerializerOptions(options);
        writeOptions.Converters.Remove(this);
        JsonSerializer.Serialize(writer, value, value.GetType(), writeOptions);
    }
}