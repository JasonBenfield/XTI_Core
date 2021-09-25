using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.IO;

namespace XTI_Core.Tests
{
    public sealed class AppDataFolderTest
    {
        private const string BasePath = @"c:\xti\appdata";

        [Test]
        public void ShouldBuildPathFromSubFolder()
        {
            var envName = "Development";
            var services = setup(envName);
            var appDataFolder = services.GetService<AppDataFolder>();
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
            var services = setup(envName);
            var appDataFolder = services.GetService<AppDataFolder>();
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
            var services = setup(envName);
            var appDataFolder = services.GetService<AppDataFolder>();
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

        private IServiceProvider setup(string envName)
        {
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", envName);
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<XtiFolder>();
                    services.AddSingleton(sp => sp.GetService<XtiFolder>().AppDataFolder());
                })
                .Build();
            var scope = host.Services.CreateScope();
            return scope.ServiceProvider;
        }
    }
}
