using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using XTI_Configuration.Extensions;
using XTI_Processes.TestAppConfig;

namespace XTI_Processes.TestApp
{
    class Program
    {
        static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.UseXtiConfiguration(hostingContext.HostingEnvironment, args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<TestOptions>(hostContext.Configuration);
                    services.Configure<NamedOptions>(hostContext.Configuration.GetSection(NamedOptions.Named));
                    services.AddHostedService<TestHostedService>();
                })
                .RunConsoleAsync();
    }
}
