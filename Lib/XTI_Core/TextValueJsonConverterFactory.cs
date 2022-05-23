using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public sealed class TextValueJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeof(TextValue).IsAssignableFrom(typeToConvert) &&
        typeToConvert.GetCustomAttribute<JsonConverterAttribute>() == null;

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter)Activator.CreateInstance
        (
            typeof(TextValueJsonConverter<>).MakeGenericType(new[] { typeToConvert }),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[0],
            culture: null
        )!;
}
