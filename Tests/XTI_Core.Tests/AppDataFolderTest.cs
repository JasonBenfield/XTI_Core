using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.IO;

namespace XTI_Core.Tests
{
    public sealed class AppDataFolderTest
    {
        [Test]
        public void ShouldBuildPathFromSubFolder()
        {
            var input = setup();
            Assert.That
            (
                input.AppDataFolder.WithSubFolder("Shared").Path(),
                Is.EqualTo
                (
                    Path.Combine(input.BasePath, "Shared")
                )
                .IgnoreCase
            );
        }

        [Test]
        public void ShouldBuildPathFromEnvironmentName()
        {
            var input = setup();
            input.HostEnv.EnvironmentName = "Development";
            Assert.That
            (
                input.AppDataFolder.WithHostEnvironment(input.HostEnv).Path(),
                Is.EqualTo
                (
                    Path.Combine(input.BasePath, input.HostEnv.EnvironmentName)
                )
                .IgnoreCase
            );
        }

        [Test]
        public void ShouldBuildPathFromSubFolders()
        {
            var input = setup();
            input.HostEnv.EnvironmentName = "Development";
            Assert.That
            (
                input.AppDataFolder
                    .WithHostEnvironment(input.HostEnv)
                    .WithSubFolder("Shared")
                    .Path(),
                Is.EqualTo
                (
                    Path.Combine(input.BasePath, input.HostEnv.EnvironmentName, "Shared")
                )
                .IgnoreCase
            );
        }

        private TestInput setup()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<AppDataFolder>();
                })
                .Build();
            var scope = host.Services.CreateScope();
            var input = new TestInput(scope.ServiceProvider);
            return input;
        }

        private class TestInput
        {
            public TestInput(IServiceProvider sp)
            {
                AppDataFolder = sp.GetService<AppDataFolder>();
                HostEnv = sp.GetService<IHostEnvironment>();
            }

            public string BasePath { get; } = @"c:\xti\appdata";
            public AppDataFolder AppDataFolder { get; }
            public IHostEnvironment HostEnv { get; }
        }
    }
}
