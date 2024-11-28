using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace XTI_Core.Extensions;

public static class CoreExtensions
{
    public static void WriteToConsole(this object data) =>
        Console.WriteLine
        (
            XtiSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true })
        );

    public static void AddConfigurationOptions<T>(this IServiceCollection services)
    where T : class, new() =>
        services.AddConfigurationOptions<T>(Options.DefaultName);

    public static void AddConfigurationOptions<T>(this IServiceCollection services, string sectionName)
    where T : class, new()
    {
        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var value = string.IsNullOrWhiteSpace(sectionName)
                ? config.Get<T>()
                : config.GetSection(sectionName).Get<T>();
            return value ?? new T();
        });
    }
}