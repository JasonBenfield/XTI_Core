using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace XTI_Core.Fakes
{
    public sealed class FakeHostEnvironment : IHostEnvironment
    {
        public FakeHostEnvironment()
        {
            ContentRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
