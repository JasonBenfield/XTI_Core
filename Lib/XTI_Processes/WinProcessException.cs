using System.Text;

namespace XTI_Processes;

public sealed class WinProcessException : Exception
{
    public WinProcessException(WinProcessResult result)
        : base(GetMessage(result))
    {
        Result = result;
    }

    private static string GetMessage(WinProcessResult result)
    {
        var message = new StringBuilder($"Process failed with exit code {result.ExitCode}");
        var joinedErrors = string.Join("\r\n", result.ErrorLines);
        if (!string.IsNullOrWhiteSpace(joinedErrors))
        {
            message.Append($"\r\n{joinedErrors}");
        }
        var joinedOutput = string.Join("\r\n", result.OutputLines);
        if (!string.IsNullOrWhiteSpace(joinedOutput))
        {
            message.Append($"Output:\r\n{joinedOutput}");
        }
        return message.ToString();
    }

    public WinProcessResult Result { get; }
}
