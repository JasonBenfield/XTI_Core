using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace XTI_Core.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder UseXtiConfiguration(this IConfigurationBuilder config, IHostEnvironment hostEnv, string appName, string appType, string[] args) =>
        config.UseXtiConfiguration(new XtiEnvironment(hostEnv.EnvironmentName), appName, appType, args);

    public static IConfigurationBuilder UseXtiConfiguration(this IConfigurationBuilder config, XtiEnvironment environment, string appName, string appType, string[] args)
    {
        config.Sources.Clear();
        var xtiFolder = new XtiFolder(environment);
        var settingsPaths = xtiFolder.SettingsPaths(appName, appType);
        foreach (var path in settingsPaths)
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
        return config;
    }
}