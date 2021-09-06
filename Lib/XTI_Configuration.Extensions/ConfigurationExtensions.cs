using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using XTI_Core;

namespace XTI_Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void UseXtiConfiguration(this IConfigurationBuilder config, IHostEnvironment hostEnv, string[] args)
        {
            config.Sources.Clear();
            var appDataFolder = new AppDataFolder().WithSubFolder("Shared");
            config
                .AddJsonFile
                (
                    appDataFolder.FilePath("appsettings.json"),
                    optional: true,
                    reloadOnChange: true
                )
                .AddJsonFile
                (
                    appDataFolder.FilePath($"appsettings.{hostEnv.EnvironmentName}.json"),
                    optional: true,
                    reloadOnChange: true
                );
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.appsettings.json");
            if (resourceStream != null)
            {
                config.AddJsonStream(resourceStream);
            }
            resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.appsettings.{hostEnv.EnvironmentName}.json");
            if (resourceStream != null)
            {
                config.AddJsonStream(resourceStream);
            }
            config
                .SetBasePath(hostEnv.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile
                (
                    $"appsettings.{hostEnv.EnvironmentName}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile
                (
                    $"appsettings.{hostEnv.EnvironmentName}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .AddEnvironmentVariables();
            if (args != null)
            {
                config.AddCommandLine(args);
            }
        }
    }
}
