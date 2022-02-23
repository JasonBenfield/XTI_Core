namespace XTI_Core;

public sealed class XtiFolder
{
    private readonly XtiEnvironment env;
    private readonly string path;

    public XtiFolder(XtiEnvironment env)
    {
        this.env = env;
        path = Environment.GetEnvironmentVariable("XTI_Dir") ?? "c:\\xti";
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "c:\\xti";
        }
    }

    public string FolderPath() => path;

    public string[] SettingsPaths(string appName, string appType)
    {
        var paths = new List<string>();
        var sharedFolder = SharedAppDataFolder();
        paths.Add(sharedFolder.FilePath("appsettings.json"));
        paths.Add(sharedFolder.FilePath("appsettings.private.json"));
        var envFolder = AppDataFolder();
        paths.Add(envFolder.FilePath("appsettings.json"));
        paths.Add(envFolder.FilePath("appsettings.private.json"));
        if (!string.IsNullOrWhiteSpace(appName) && !string.IsNullOrWhiteSpace(appType))
        {
            var appFolder = AppDataFolder(appName, appType);
            paths.Add(appFolder.FilePath("appsettings.json"));
            paths.Add(appFolder.FilePath("appsettings.private.json"));
        }
        paths.Add(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
        paths.Add(Path.Combine(AppContext.BaseDirectory, $"appsettings.{env.EnvironmentName}.json"));
        return paths.ToArray();
    }

    public AppDataFolder SharedAppDataFolder()
        => getAppDataFolder();

    public AppDataFolder AppDataFolder()
        => getAppDataFolder()
            .WithHostEnvironment(env);

    public AppDataFolder AppDataFolder(string appName, string appType)
        => getAppDataFolder()
            .WithHostEnvironment(env)
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
            env.EnvironmentName,
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
            env.EnvironmentName,
            $"{getAppType(appType)}s",
            getAppName(appName),
            versionKey
        );

    private static string getAppName(string appName)
        => appName.Replace(" ", "");

    private static string getAppType(string appType)
        => appType.Replace(" ", "");
}
