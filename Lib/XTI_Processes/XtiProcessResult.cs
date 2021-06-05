using System.Text.Json;
using System.Text.RegularExpressions;

namespace XTI_Processes
{
    public sealed class XtiProcessResult
    {
        private readonly WinProcessResult result;

        public XtiProcessResult(WinProcessResult result)
        {
            this.result = result;
        }

        public int ExitCode { get => result.ExitCode; }

        private static readonly Regex dataRegex = new Regex("<data>(.*)<\\/data>");

        public T Data<T>()
        {
            T data;
            var output = joinedOutput();
            var m = dataRegex.Match(output);
            if (m.Success)
            {
                data = JsonSerializer.Deserialize<T>(m.Groups[1].Value);
            }
            else
            {
                data = default;
            }
            return data;
        }

        public string Error()
        {
            return null;
        }

        private string joinedOutput() => string.Join("\r\n", result.OutputLines);
    }
}
