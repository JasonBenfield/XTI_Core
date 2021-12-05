using Microsoft.Extensions.Hosting;

namespace XTI_Core;

public sealed class AppDataFolder
{
    private readonly string path;
    private readonly List<string> subFolderNames = new List<string>();

    internal AppDataFolder(string path)
        : this(path, new string[] { })
    {
    }

    private AppDataFolder(string path, IEnumerable<string> subFolderNames)
    {
        this.path = path;
        this.subFolderNames.AddRange(subFolderNames);
    }

    public string Path()
        => System.IO.Path.Combine
        (
            new[]
            {
                    Environment.GetEnvironmentVariable("XTI_Dir") ?? "c:\\xti",
                    "AppData"
            }
            .Union(subFolderNames)
            .ToArray()
        );

    public string FilePath(string fileName) => System.IO.Path.Combine(Path(), fileName);

    public AppDataFolder WithSubFolder(string name)
        => new AppDataFolder(path, subFolderNames.Union(new[] { name }));

    internal AppDataFolder WithHostEnvironment(IHostEnvironment hostEnv)
        => WithSubFolder(hostEnv.EnvironmentName);

    public void TryCreate()
    {
        var path = Path();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}