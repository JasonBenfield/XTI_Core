using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTI_Processes
{
    public sealed class DotnetRunProcess
    {
        private readonly string project;
        private readonly OptionDictionary config = new OptionDictionary();
        private string environment;

        public DotnetRunProcess()
            : this("")
        {
        }

        public DotnetRunProcess(string project)
        {
            this.project = project ?? "";
            UseDevelopmentEnvironment();
        }

        public DotnetRunProcess UseEnvironment(string environment)
        {
            environment = environment?.Trim() ?? "";
            if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                return UseDevelopmentEnvironment();
            }
            if (environment.Equals("Test", StringComparison.OrdinalIgnoreCase))
            {
                return UseTestEnvironment();
            }
            if (environment.Equals("Staging", StringComparison.OrdinalIgnoreCase))
            {
                return UseStagingEnvironment();
            }
            if (environment.Equals("Production", StringComparison.OrdinalIgnoreCase))
            {
                return UseProductionEnvironment();
            }
            throw new NotSupportedException($"Environment '{environment}' is not supported");
        }

        public DotnetRunProcess UseDevelopmentEnvironment() => useEnvironemnt("Development");

        public DotnetRunProcess UseTestEnvironment() => useEnvironemnt("Test");

        public DotnetRunProcess UseStagingEnvironment() => useEnvironemnt("Staging");

        public DotnetRunProcess UseProductionEnvironment() => useEnvironemnt("Production");

        private DotnetRunProcess useEnvironemnt(string environment)
        {
            this.environment = environment;
            return this;
        }

        public DotnetRunProcess AddConfigOptions(object options, string name = "")
        {
            config.Add(options, name);
            return this;
        }

        public async Task<WinProcessResult> Run()
        {
            var process = new WinProcess("dotnet");
            process.UseArgumentNameDelimiter("");
            process.AddArgument("run");
            process.UseArgumentNameDelimiter("--");
            process.UseArgumentValueDelimiter(" ");
            process.AddArgument("project", new Quoted(project));
            process.AddArgument("environment", environment);
            var dict = config.ToDictionary();
            foreach (var key in dict.Keys)
            {
                process.AddArgument(key, new Quoted(dict[key]));
            }
            var result = await process.Run();
            return result;
        }
    }
}
