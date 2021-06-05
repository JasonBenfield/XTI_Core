using System;
using System.Threading.Tasks;

namespace XTI_Processes
{
    public sealed class XtiProcess
    {
        private readonly string fileName;
        private readonly OptionDictionary config = new OptionDictionary();
        private string environment;

        public XtiProcess(string fileName)
        {
            this.fileName = fileName;
            UseDevelopmentEnvironment();
        }

        public XtiProcess UseEnvironment(string environment)
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

        public XtiProcess UseDevelopmentEnvironment() => useEnvironemnt("Development");

        public XtiProcess UseTestEnvironment() => useEnvironemnt("Test");

        public XtiProcess UseStagingEnvironment() => useEnvironemnt("Staging");

        public XtiProcess UseProductionEnvironment() => useEnvironemnt("Production");

        private XtiProcess useEnvironemnt(string environment)
        {
            this.environment = environment;
            return this;
        }

        public XtiProcess AddConfigOptions(object options, string name = "")
        {
            config.Add(options, name);
            return this;
        }

        public async Task<XtiProcessResult> Run()
        {
            var process = new WinProcess(fileName);
            process.UseArgumentNameDelimiter("--");
            process.UseArgumentValueDelimiter(" ");
            process.AddArgument("environment", environment);
            var dict = config.ToDictionary();
            foreach (var key in dict.Keys)
            {
                process.AddArgument(key, new Quoted(dict[key]));
            }
            var result = await process.Run();
            return new XtiProcessResult(result);
        }
    }
}
