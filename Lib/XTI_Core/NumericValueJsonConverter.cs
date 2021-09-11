using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core
{
    public sealed class NumericValueJsonConverter : JsonConverter<NumericValue>
    {
        public override bool CanConvert(Type typeToConvert)
            => typeof(NumericValue).IsAssignableFrom(typeToConvert);

        public override NumericValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token");
            }
            int value = 0;
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected PropertyName token");
                }
                var propName = reader.GetString();
                reader.Read();
                if (propName == "Value")
                {
                    value = reader.GetInt32();
                }
            }
            var valuesField = typeToConvert.GetField("Values", BindingFlags.Static | BindingFlags.Public);
            var values = valuesField.GetValue(typeToConvert);
            var valueMethod = valuesField.FieldType.GetMethod("Value", new[] { typeof(int) });
            var numericValue = valueMethod.Invoke(values, new[] { (object)value });
            return (NumericValue)numericValue;
        }

        public override void Write(Utf8JsonWriter writer, NumericValue value, JsonSerializerOptions options)
        {
            var writeOptions = new JsonSerializerOptions(options);
            writeOptions.Converters.Remove(this);
            JsonSerializer.Serialize(writer, value, value.GetType(), writeOptions);
        }
    }
}
