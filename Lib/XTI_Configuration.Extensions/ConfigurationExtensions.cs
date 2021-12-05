using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using XTI_Core;

namespace XTI_Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void UseXtiConfiguration(this IConfigurationBuilder config, IHostEnvironment hostEnv, string[] args)
    {
        config.Sources.Clear();
        var xtiFolder = new XtiFolder(hostEnv);
        var sharedSettingsPaths = xtiFolder.SharedSettingsPaths();
        foreach (var path in sharedSettingsPaths)
        {
            config.AddJsonFile
            (
                path,
                optional: true,
                reloadOnChange: true
            );
        }
        var appSettingsPaths = new[]
        {
                Path.Combine(AppContext.BaseDirectory, "appsettings.json"),
                Path.Combine(AppContext.BaseDirectory, $"appsettings.{hostEnv.EnvironmentName}.json")
            };
        foreach (var path in appSettingsPaths)
        {
            config.AddJsonFile
            (
                path,
                optional: true,
                reloadOnChange: true
            );
        }
        config.AddEnvironmentVariables();
        if (args != null)
        {
            config.AddCommandLine(args);
        }
    }
}