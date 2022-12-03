using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

public static class XtiSerializer
{
    public static string Serialize(object? obj, JsonSerializerOptions? options = null) =>
        JsonSerializer.Serialize(obj, options ?? DefaultOptions());

    public static T Deserialize<T>(string str, JsonSerializerOptions? options = null)
        where T : new() =>
        Deserialize<T>(str, () => new T(), options);

    public static T Deserialize<T>(string str, Func<T> ifnull, JsonSerializerOptions? options = null) =>
        JsonSerializer.Deserialize<T>(str, options ?? DefaultOptions()) ?? ifnull();

    private static JsonSerializerOptions DefaultOptions() =>
        new JsonSerializerOptions().AddCoreConverters();

    public static JsonSerializerOptions AddCoreConverters(this JsonSerializerOptions options)
    {
        options.AddConverter<NullStringAsEmptyJsonConverter>();
        options.AddConverter<NumericValueJsonConverterFactory>();
        options.AddConverter<TextValueJsonConverterFactory>();
        options.AddConverter<TimeSpanJsonConverter>();
        options.AddConverter<DictionaryStringObjectJsonConverter>();
        return options;
    }

    public static JsonSerializerOptions AddConverter<T>(this JsonSerializerOptions options)
        where T : JsonConverter, new()
    {
        if (!options.Converters.OfType<T>().Any())
        {
            options.Converters.Add(new T());
        }
        return options;
    }
}