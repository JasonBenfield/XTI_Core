using Microsoft.Extensions.Hosting;

namespace XTI_Core;

public sealed class XtiFolder
{
    private readonly IHostEnvironment hostEnv;
    private readonly string path;

    public XtiFolder(IHostEnvironment hostEnv)
    {
        this.hostEnv = hostEnv;
        path = Environment.GetEnvironmentVariable("XTI_Dir") ?? "c:\\xti";
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "c:\\xti";
        }
    }

    public string FolderPath() => path;

    public string[] SharedSettingsPaths()
    {
        var sharedAppDataFolder = SharedAppDataFolder();
        return new[]
        {
                sharedAppDataFolder.FilePath("appsettings.json"),
                sharedAppDataFolder.FilePath($"appsettings.{hostEnv.EnvironmentName}.json")
            };
    }

    public AppDataFolder SharedAppDataFolder()
        => getAppDataFolder()
            .WithSubFolder("Shared");

    public AppDataFolder AppDataFolder()
        => getAppDataFolder()
            .WithHostEnvironment(hostEnv);

    public AppDataFolder AppDataFolder(string appName, string appType)
        => getAppDataFolder()
            .WithHostEnvironment(hostEnv)
            .WithSubFolder($"{getAppType(appType)}s")
            .WithSubFolder(getAppName(appName));

    private AppDataFolder getAppDataFolder()
        => new AppDataFolder(Path.Combine(path, "AppData"));

    public string ToolsPath() => Path.Combine(path, "Tools");

    public string InstallPath(string appName, string appType)
        => InstallPath(appName, appType, "");

    public string InstallPath(string appName, string appType, string versionKey)
    {
        var installPath = Path.Combine
        (
            path,
            "Apps",
            hostEnv.EnvironmentName,
            $"{getAppType(appType)}s",
            getAppName(appName)
        );
        if (!string.IsNullOrWhiteSpace(versionKey))
        {
            installPath = Path.Combine(installPath, versionKey);
        }
        return installPath;
    }

    public string PublishPath(string appName, string appType, string versionKey)
        => Path.Combine
        (
            path,
            "Published",
            hostEnv.EnvironmentName,
            $"{getAppType(appType)}s",
            getAppName(appName),
            versionKey
        );

    private static string getAppName(string appName)
        => appName.Replace(" ", "");

    private static string getAppType(string appType)
        => appType.Replace(" ", "");
}
