using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XTI_Core.Extensions;

public sealed class XtiHost
{
    private readonly IServiceProvider sp;
    private IServiceScope? scope;

    internal XtiHost(IServiceProvider sp, IConfiguration configuration)
    {
        this.sp = sp;
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public T GetRequiredService<T>()
        where T : notnull =>
            Scope().GetRequiredService<T>();

    public T? GetService<T>() => Scope().GetService<T>();

    public IServiceProvider Scope() => (scope ??= sp.CreateScope()).ServiceProvider;

    public IServiceProvider NewScope() => sp.CreateScope().ServiceProvider;
}