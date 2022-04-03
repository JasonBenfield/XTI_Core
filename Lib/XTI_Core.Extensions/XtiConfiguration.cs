using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace XTI_Core.Extensions;

public sealed class XtiConfiguration : IXtiConfiguration
{
    public XtiConfiguration(IConfiguration source)
    {
        Source = source;
    }

    public IConfiguration Source { get; }

    public string this[string key] { get => Source[key]; set => Source[key] = value; }

    public IEnumerable<IConfigurationSection> GetChildren() => Source.GetChildren();

    public IChangeToken GetReloadToken() => Source.GetReloadToken();

    public IConfigurationSection GetSection(string key) => Source.GetSection(key);
}