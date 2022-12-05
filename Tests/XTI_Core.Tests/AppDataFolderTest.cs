using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using XTI_Core.Extensions;

namespace XTI_Core.Tests;

public sealed class AppDataFolderTest
{
    private const string BasePath = @"c:\xti\appdata";

    [Test]
    public void ShouldBuildPathFromSubFolder()
    {
        var envName = "Development";
        var services = Setup(envName);
        var appDataFolder = services.GetRequiredService<AppDataFolder>();
        Assert.That
        (
            appDataFolder.WithSubFolder("Test").Path(),
            Is.EqualTo
            (
                Path.Combine(BasePath, envName, "Test")
            )
            .IgnoreCase
        );
    }

    [Test]
    public void ShouldBuildPathFromEnvironmentName()
    {
        var envName = "Development";
        var services = Setup(envName);
        var appDataFolder = services.GetRequiredService<AppDataFolder>();
        Assert.That
        (
            appDataFolder.Path(),
            Is.EqualTo
            (
                Path.Combine(BasePath, envName)
            )
            .IgnoreCase
        );
    }

    [Test]
    public void ShouldBuildPathFromSubFolders()
    {
        var envName = "Development";
        var services = Setup(envName);
        var appDataFolder = services.GetRequiredService<AppDataFolder>();
        Assert.That
        (
            appDataFolder
                .WithSubFolder("Test")
                .WithSubFolder("SubFolder")
                .Path(),
            Is.EqualTo
            (
                Path.Combine(BasePath, envName, "Test", "Subfolder")
            )
            .IgnoreCase
        );
    }

    private IServiceProvider Setup(string envName)
    {
        var hostBuilder = new XtiHostBuilder(XtiEnvironment.Parse(envName));
        hostBuilder.Services.AddSingleton(sp => sp.GetRequiredService<XtiFolder>().AppDataFolder());
        var host = hostBuilder.Build();
        return host.Scope();
    }
}