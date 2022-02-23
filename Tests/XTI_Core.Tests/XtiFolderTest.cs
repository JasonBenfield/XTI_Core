using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using XTI_Core.Extensions;

namespace XTI_Core.Tests;

sealed class XtiFolderTest
{
    private const string XtiDir = @"c:\xti";

    [Test]
    public void ShouldReturnXtiFolder()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.FolderPath(),
            Is.EqualTo(XtiDir).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnToolsPath()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.ToolsPath(),
            Is.EqualTo(Path.Combine(XtiDir, "Tools")).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnSharedAppDataFolder()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.SharedAppDataFolder().Path(),
            Is.EqualTo(Path.Combine(XtiDir, "AppData")).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnBaseAppDataFolder()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.AppDataFolder().Path(),
            Is.EqualTo(Path.Combine(XtiDir, "AppData", envName)).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnAppDataFolder()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.AppDataFolder("Test App", "Web App").Path(),
            Is.EqualTo(Path.Combine(XtiDir, "AppData", envName, "WebApps", "TestApp")).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnInstallPath()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.InstallPath("Test App", "Web App"),
            Is.EqualTo(Path.Combine(XtiDir, "Apps", envName, "WebApps", "TestApp")).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnInstallPathForVersion()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.InstallPath("Test App", "Web App", "Current"),
            Is.EqualTo(Path.Combine(XtiDir, "Apps", envName, "WebApps", "TestApp", "Current")).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnPublishPath()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        Assert.That
        (
            xtiFolder.PublishPath("Test App", "Web App", "Current"),
            Is.EqualTo(Path.Combine(XtiDir, "Published", envName, "WebApps", "TestApp", "Current")).IgnoreCase
        );
    }

    [Test]
    public void ShouldReturnSettingsPaths()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        var settingsPaths = xtiFolder.SettingsPaths("Hub", "WebApp");
        Console.WriteLine(string.Join("\r\n", settingsPaths));
    }

    [Test]
    public void ShouldReturnSettingsPaths_WhenAppNameAndAppTypeAreBlank()
    {
        const string envName = "Development";
        var services = setup(envName);
        var xtiFolder = services.GetRequiredService<XtiFolder>();
        var settingsPaths = xtiFolder.SettingsPaths("", "");
        Console.WriteLine(string.Join("\r\n", settingsPaths));
    }

    private IServiceProvider setup(string envName)
    {
        Environment.SetEnvironmentVariable("XTI_Dir", XtiDir);
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", envName);
        var builder = new XtiHostBuilder();
        return builder.Build().Scope();
    }
}