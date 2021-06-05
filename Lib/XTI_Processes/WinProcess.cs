using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace XTI_Processes
{
    public sealed class WinProcess
    {
        private readonly string fileName;
        private readonly List<string> args = new List<string>();

        private string nameDelimiter = "";
        private string valueDelimiter = " ";

        public WinProcess(string fileName)
        {
            this.fileName = fileName;
        }

        public WinProcess UseArgumentNameDelimiter(string nameDelimiter)
        {
            this.nameDelimiter = nameDelimiter;
            return this;
        }

        public WinProcess UseArgumentValueDelimiter(string valueDelimiter)
        {
            this.valueDelimiter = valueDelimiter;
            return this;
        }

        public WinProcess AddArgument(Quoted arg)
            => AddArgument(arg?.Value());

        public WinProcess AddArgument(string arg)
            => AddArgument(new WinArgument("", arg, "", ""));

        public WinProcess AddArgument(string name, Quoted value)
            => AddArgument(name, value?.Value());

        public WinProcess AddArgument(string name, string value)
            => AddArgument(new WinArgument(nameDelimiter, name, valueDelimiter, value));

        internal WinProcess AddArgument(WinArgument arg)
        {
            var output = arg.Output();
            if (!string.IsNullOrWhiteSpace(output))
            {
                args.Add(output);
            }
            return this;
        }

        public async Task<WinProcessResult> Run()
        {
            using var process = new Process();
            var psi = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            if (args.Any())
            {
                psi.Arguments = string.Join(" ", args);
            }
            process.StartInfo = psi;
            var started = process.Start();
            var outputLines = new List<string>();
            var errorLines = new List<string>();
            while (!process.StandardOutput.EndOfStream || !process.StandardError.EndOfStream)
            {
                var outputLine = process.StandardOutput.ReadLine();
                if (!string.IsNullOrWhiteSpace(outputLine))
                {
                    outputLines.Add(outputLine.Trim());
                }
                var errorLine = process.StandardError.ReadLine();
                if (!string.IsNullOrWhiteSpace(errorLine))
                {
                    errorLines.Add(errorLine.Trim());
                }
            }
            await process.WaitForExitAsync();
            var result = new WinProcessResult
            (
                process.ExitCode,
                outputLines.ToArray(),
                errorLines.ToArray()
            );
            return result;
        }
    }
}
