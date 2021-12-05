using System.Text.Json;

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
        if (!options.Converters.OfType<NullStringAsEmptyJsonConverter>().Any())
        {
            options.Converters.Add(new NullStringAsEmptyJsonConverter());
        }
        if (!options.Converters.OfType<NumericValueJsonConverter>().Any())
        {
            options.Converters.Add(new NumericValueJsonConverter());
        }
        if (!options.Converters.OfType<TimeJsonConverter>().Any())
        {
            options.Converters.Add(new TimeJsonConverter());
        }
        if (!options.Converters.OfType<TimeRangeJsonConverter>().Any())
        {
            options.Converters.Add(new TimeRangeJsonConverter());
        }
        if (!options.Converters.OfType<TimeSpanJsonConverter>().Any())
        {
            options.Converters.Add(new TimeSpanJsonConverter());
        }
        return options;
    }
}