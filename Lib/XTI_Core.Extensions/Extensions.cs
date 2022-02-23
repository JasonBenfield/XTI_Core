using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace XTI_Core.Extensions;

public static class Extensions
{
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