using System.Diagnostics;

namespace XTI_Processes;

public sealed class WinProcess : IProcess
{
    private readonly string fileName;
    private readonly List<string> args = new List<string>();

    private string nameDelimiter = "";
    private string valueDelimiter = " ";

    private readonly List<string> outputLines = new List<string>();
    private readonly List<string> errorLines = new List<string>();

    private bool outputToConsole = false;
    private string workingDirectory = "";

    public WinProcess(string fileName)
    {
        this.fileName = fileName;
    }

    public event EventHandler<DataReceivedEventArgs>? OutputDataReceived;
    public event EventHandler<DataReceivedEventArgs>? ErrorDataReceived;

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
        => AddArgument(arg.Value());

    public WinProcess AddArgument(string arg)
        => AddArgument(new WinArgument(nameDelimiter, arg, "", ""));

    public WinProcess AddArgument(string name, Quoted value)
        => AddArgument(name, value.Value());

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

    public WinProcess WriteOutputToConsole()
    {
        outputToConsole = true;
        return this;
    }

    public WinProcess SetWorkingDirectory(string workingDirectory)
    {
        this.workingDirectory = workingDirectory;
        return this;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public async Task<WinProcessResult> Run()
    {
        using var process = new Process();
        var psi = new ProcessStartInfo(fileName)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            LoadUserProfile = true
        };
        if (args.Any())
        {
            psi.Arguments = string.Join(" ", args);
        }
        if (!string.IsNullOrWhiteSpace(workingDirectory))
        {
            psi.WorkingDirectory = workingDirectory;
        }
        process.StartInfo = psi;
        process.OutputDataReceived += Process_OutputDataReceived;
        outputLines.Clear();
        process.ErrorDataReceived += Process_ErrorDataReceived;
        errorLines.Clear();
        var started = process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();
        var result = new WinProcessResult
        (
            process.ExitCode,
            outputLines.ToArray(),
            errorLines.ToArray()
        );
        return result;
    }

    private void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            if (outputToConsole)
            {
                Console.WriteLine(e.Data);
            }
            ErrorDataReceived?.Invoke(this, new DataReceivedEventArgs(e.Data));
            errorLines.Add(e.Data);
        }
    }

    private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            if (outputToConsole)
            {
                Console.Error.WriteLine(e.Data);
            }
            OutputDataReceived?.Invoke(this, new DataReceivedEventArgs(e.Data));
            outputLines.Add(e.Data);
        }
    }

    public string CommandText()
    {
        var commandText = fileName;
        if (args.Any())
        {
            var argsText = string.Join(" ", args);
            commandText += $" {argsText}";
        }
        return commandText;
    }
}
