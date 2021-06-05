using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using XTI_Processes.TestAppConfig;
using XTI_Tool;

namespace XTI_Processes.TestApp
{
    public sealed class TestHostedService : IHostedService
    {
        private readonly IServiceProvider services;

        public TestHostedService(IServiceProvider services)
        {
            this.services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting...");
            using var scope = services.CreateScope();
            var testOptions = scope.ServiceProvider.GetService<IOptions<TestOptions>>().Value;
            var namedOptions = scope.ServiceProvider.GetService<IOptions<NamedOptions>>().Value;
            var hostEnv = scope.ServiceProvider.GetService<IHostEnvironment>();
            Environment.ExitCode = testOptions.ExitCode;
            var data = new TestAppData
            {
                Output = testOptions.Output,
                Environment = hostEnv.EnvironmentName,
                NamedOptions = namedOptions
            };
            new XtiProcessData().Output(data);
            Console.WriteLine("Stopping...");
            var lifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            lifetime.StopApplication();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
