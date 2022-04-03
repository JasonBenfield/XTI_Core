namespace XTI_Processes;

public sealed class CmdProcess : IProcess
{
    private readonly IProcess otherProcess;

    private bool outputToConsole = false;
    private string workingDirectory = "";

    public CmdProcess(IProcess otherProcess)
    {
        this.otherProcess = otherProcess;
    }

    public string CommandText()
    {
        var process = createProcess();
        return process.CommandText();
    }

    public CmdProcess WriteOutputToConsole()
    {
        outputToConsole = true;
        return this;
    }

    public CmdProcess SetWorkingDirectory(string workingDirectory)
    {
        this.workingDirectory = workingDirectory;
        return this;
    }

    public async Task<WinProcessResult> Run()
    {
        var process = createProcess();
        var result = await process.Run();
        return result;
    }

    private WinProcess createProcess()
    {
        var process = new WinProcess("cmd");
        process.SetWorkingDirectory(workingDirectory);
        if (outputToConsole)
        {
            process.WriteOutputToConsole();
        }
        process.UseArgumentNameDelimiter("/");
        process.AddArgument("C");
        process.UseArgumentNameDelimiter("");
        process.AddArgument(otherProcess.CommandText());
        return process;
    }
}