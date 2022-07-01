using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace XTI_Core.Extensions;

public sealed class XtiHostBuilder
{
    public XtiHostBuilder()
        : this(XtiEnvironment.Development)
    {
    }

    public XtiHostBuilder(XtiEnvironment environment)
        : this(environment, "", "", new string[0])
    {
    }

    public XtiHostBuilder(string appName, string appType)
        : this(XtiEnvironment.Development, appName, appType)
    {
    }

    public XtiHostBuilder(XtiEnvironment environment, string appName, string appType)
        : this(environment, appName, appType, new string[0])
    {
    }

    public XtiHostBuilder(XtiEnvironment xtiEnv, string appName, string appType, string[] args)
    {
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", xtiEnv.EnvironmentName);
        Configuration = new ConfigurationBuilder();
        Configuration.UseXtiConfiguration(xtiEnv, appName, appType, args);
        Services = new ServiceCollection();
        Services.AddSingleton<IHostEnvironment>(new HostingEnvironment
        {
            EnvironmentName = xtiEnv.EnvironmentName
        });
        Services.AddSingleton(_ => Configuration.Build());
        Services.AddSingleton(sp => (IConfiguration)sp.GetRequiredService<IConfigurationRoot>());
        Services.AddSingleton<XtiFolder>();
        Services.AddSingleton(_ => xtiEnv);
    }

    public IConfigurationBuilder Configuration { get; }

    public IServiceCollection Services { get; }

    public Func<IServiceProvider, IConfiguration> ConfigurationAccessor() =>
        sp => sp.GetRequiredService<IConfigurationRoot>();

    public XtiHost Build()
    {
        var sp = Services.BuildServiceProvider();
        var configuration = sp.GetRequiredService<IConfiguration>();
        return new XtiHost(sp, configuration);
    }
}