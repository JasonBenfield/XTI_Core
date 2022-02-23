using Microsoft.Extensions.Configuration;

namespace XTI_Core.Extensions;

public interface IXtiConfiguration : IConfiguration
{
    IConfiguration Source { get; }
}