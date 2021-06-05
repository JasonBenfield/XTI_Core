using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using XTI_Processes.TestAppConfig;

namespace XTI_Processes.Tests
{
    public sealed class DotnetRunProcessTest
    {
        [Test]
        public async Task ShouldReturnExitCode()
        {
            var process = createProcess();
            const int exitCode = 999;
            process.AddConfigOptions(new TestOptions { ExitCode = exitCode });
            var result = await run(process);
            Assert.That(result.ExitCode, Is.EqualTo(exitCode), "Should return exit code");
        }

        [Test]
        public async Task ShouldDefaultToDevelopmentEnvironment()
        {
            var process = createProcess();
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Development"), "Should set Development environment");
        }

        [Test]
        public async Task ShouldSetStagingEnvironment()
        {
            var process = createProcess();
            process.UseStagingEnvironment();
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Staging"), "Should set Staging environment");
        }

        [Test]
        public async Task ShouldSetDevelopmentEnvironment()
        {
            var process = createProcess();
            process.UseDevelopmentEnvironment();
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Development"), "Should set Development environment");
        }

        [Test]
        public async Task ShouldSetProductionEnvironment()
        {
            var process = createProcess();
            process.UseProductionEnvironment();
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Production"), "Should set Production environment");
        }

        [Test]
        public async Task ShouldSetTestEnvironment()
        {
            var process = createProcess();
            process.UseTestEnvironment();
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo("Test"), "Should set Test environment");
        }

        [Test]
        [TestCase("Development"), TestCase("Test"), TestCase("Staging"), TestCase("Production")]
        public async Task ShouldSetEnvironment(string environment)
        {
            var process = createProcess();
            process.UseEnvironment(environment);
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.Environment, Is.EqualTo(environment), $"Should set {environment} environment");
        }

        [Test]
        public async Task ShouldAddNamedConfig()
        {
            var process = createProcess();
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
            var result = await run(process);
            var data = result.Data<TestAppData>();
            Assert.That(data?.NamedOptions?.Value1, Is.EqualTo(value1), "Should add named config options");
            Assert.That
            (
                data?.NamedOptions?.Nested?.SomeTime,
                Is.EqualTo(someTime),
                "Should add named config options"
            );
        }

        private static DotnetRunProcess createProcess()
        {
            var project = getTestAppProject();
            var process = new DotnetRunProcess(project);
            return process;
        }

        private static string getTestAppProject()
        {
            return Path.Combine
            (
                Path.GetFullPath(Path.Combine(TestContext.CurrentContext.WorkDirectory, "..", "..", "..", "..")),
                "XTI_Processes.TestApp"
            );
        }

        private static async Task<XtiProcessResult> run(DotnetRunProcess process)
        {
            var winProcessResult = await process.Run();
            var result = new XtiProcessResult(winProcessResult);
            return result;
        }

    }
}
