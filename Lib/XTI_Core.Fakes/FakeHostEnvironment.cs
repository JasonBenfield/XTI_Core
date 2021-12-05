using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace XTI_Core.Fakes;

public sealed class FakeHostEnvironment : IHostEnvironment
{
    private string environmentName = "";
    private string applicationName = "";
    private string contentRootPath = "";
    private IFileProvider contentRootFileProvider = new NullFileProvider();

    public FakeHostEnvironment()
    {
        ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
    }

    public string EnvironmentName
    {
        get => environmentName;
        set => environmentName = value ?? "";
    }

    public string ApplicationName
    {
        get => applicationName;
        set => applicationName = value ?? "";
    }

    public string ContentRootPath
    {
        get => contentRootPath;
        set => contentRootPath = value ?? "";
    }

    public IFileProvider ContentRootFileProvider
    {
        get => contentRootFileProvider;
        set => contentRootFileProvider = value ?? new NullFileProvider();
    }
}