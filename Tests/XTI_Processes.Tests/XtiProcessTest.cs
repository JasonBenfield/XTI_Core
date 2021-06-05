using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTI_Processes.TestAppConfig;

namespace XTI_Processes.Tests
{
    public sealed class XtiProcessTest
    {
        [Test]
        public async Task ShouldReturnExitCode()
        {
            string fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            const int exitCode = 999;
            process.AddConfigOptions(new TestOptions { ExitCode = exitCode });
            var result = await process.Run();
            Assert.That(result.ExitCode, Is.EqualTo(exitCode), "Should return exit code");
        }

        [Test]
        public async Task ShouldReturnData()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            const string output = "Test Output";
            process.AddConfigOptions(new TestOptions { Output = output });
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Output, Is.EqualTo(output), "Should return data");
        }

        [Test]
        public async Task ShouldDefaultToDevelopmentEnvironment()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Development"), "Should set Development environment");
        }

        [Test]
        public async Task ShouldSetStagingEnvironment()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            process.UseStagingEnvironment();
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Staging"), "Should set Staging environment");
        }

        [Test]
        public async Task ShouldSetDevelopmentEnvironment()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            process.UseDevelopmentEnvironment();
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Development"), "Should set Development environment");
        }

        [Test]
        public async Task ShouldSetProductionEnvironment()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            process.UseProductionEnvironment();
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Production"), "Should set Production environment");
        }

        [Test]
        public async Task ShouldSetTestEnvironment()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            process.UseTestEnvironment();
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Test"), "Should set Test environment");
        }

        [Test]
        [TestCase("Development"), TestCase("Test"), TestCase("Staging"), TestCase("Production")]
        public async Task ShouldSetEnvironment(string environment)
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            process.UseEnvironment(environment);
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo(environment), $"Should set {environment} environment");
        }

        [Test]
        public async Task ShouldAddNamedConfig()
        {
            var fileName = getTestAppFileName();
            var process = new XtiProcess(fileName);
            const string value1 = "Test Value 1";
            var someTime = new DateTimeOffset(2021, 06, 02, 21, 28, 05, new TimeSpan());
            process.AddConfigOptions
            (
                new NamedOptions
                {
                    Value1 = value1,
                    Nested = new NestedOptions
                    {
                        SomeTime = someTime
                    }
                },
                NamedOptions.Named
            );
            var result = await process.Run();
            var data = result.Data<TestAppData>();
            Assert.That(data?.NamedOptions?.Value1, Is.EqualTo(value1), "Should add named config options");
            Assert.That
            (
                data?.NamedOptions?.Nested?.SomeTime,
                Is.EqualTo(someTime),
                "Should add named config options"
            );
        }

        private static string getTestAppFileName()
        {
            return Path.Combine
            (
                Path.GetFullPath(Path.Combine(TestContext.CurrentContext.WorkDirectory, "..", "..")),
                "TestApp",
                "XTI_Processes.TestApp.exe"
            );
        }

    }
}
