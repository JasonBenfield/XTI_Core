using System.Text.Json;
using System.Text.RegularExpressions;

namespace XTI_Processes;

public sealed partial record WinProcessResult(int ExitCode, string[] OutputLines, string[] ErrorLines)
{
    public T Data<T>()
        where T : new() =>
        Data(() => new T());

    public T Data<T>(Func<T> ifnull)
    {
        T data;
        var output = joinedOutput();
        var m = DataRegex().Match(output);
        if (m.Success)
        {
            data = JsonSerializer.Deserialize<T>(m.Groups[1].Value) ?? ifnull();
        }
        else
        {
            data = ifnull();
        }
        return data;
    }

    private string joinedOutput() => string.Join("\r\n", OutputLines);

    public WinProcessResult EnsureExitCodeIsZero() => EnsureExitCodeIsValid(ec => ec == 0);

    public WinProcessResult EnsureExitCodeIsValid(Func<int, bool> isValid)
    {
        if (!isValid(ExitCode))
        {
            ThrowException();
        }
        return this;
    }

    public WinProcessResult EnsureNoErrorOutput()
    {
        if (ErrorLines.Length > 0)
        {
            ThrowException();
        }
        return this;
    }

    private void ThrowException() => throw new WinProcessException(this);

    [GeneratedRegex("<data>(.*)<\\/data>")]
    private static partial Regex DataRegex();
}