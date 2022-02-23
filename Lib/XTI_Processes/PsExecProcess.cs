namespace XTI_Processes;

public sealed class PsExecProcess : IProcess
{
    private readonly string server;
    private readonly IProcess remoteProcess;

    public PsExecProcess(string server, IProcess remoteProcess)
    {
        if (!server.StartsWith("\\\\"))
        {
            server = $"\\\\{server}";
        }
        this.server = server;
        this.remoteProcess = remoteProcess;
    }

    public string CommandText()
    {
        var process = createProcess();
        return process.CommandText();
    }

    public async Task<WinProcessResult> Run()
    {
        var process = createProcess();
        var result = await process.Run();
        return result;
    }

    private bool outputToConsole = false;

    public PsExecProcess WriteOutputToConsole()
    {
        outputToConsole = true;
        return this;
    }

    private WinProcess createProcess()
    {
        var process = new WinProcess("psexec");
        if (outputToConsole)
        {
            process.WriteOutputToConsole();
        }
        process.UseArgumentNameDelimiter("");
        process.AddArgument(server);
        process.AddArgument(remoteProcess.CommandText());
        return process;
    }
}