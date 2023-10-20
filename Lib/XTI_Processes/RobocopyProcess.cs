namespace XTI_Processes;

public sealed class RobocopyProcess
{
    private readonly string source;
    private readonly string target;
    private string pattern = "";
    private readonly List<WinArgument> options = new List<WinArgument>();
    private readonly List<string> attributesToAdd = new List<string>();
    private string workingDirectory = "";

    public RobocopyProcess(string source, string target)
    {
        this.source = source;
        this.target = target;
    }

    public RobocopyProcess Pattern(string pattern)
    {
        this.pattern = pattern;
        return this;
    }

    public RobocopyProcess CopySubdirectoriesIncludingEmpty() => AddOption("e");

    public RobocopyProcess Purge() => AddOption("purge");

    public RobocopyProcess NoJobHeader() => AddOption("njh");

    public RobocopyProcess NoJobSummary() => AddOption("njs");

    public RobocopyProcess NoDirectoryLogging() => AddOption("ndl");

    public RobocopyProcess NoFileLogging() => AddOption("nfl");

    public RobocopyProcess NoFileSizeLogging() => AddOption("ns");

    public RobocopyProcess NoFileClassLogging() => AddOption("nc");

    public RobocopyProcess NoProgressDisplayed() => AddOption("np");

    public RobocopyProcess MoveFiles() => AddOption("mov");

    public RobocopyProcess MoveFilesAndDirectories() => AddOption("move");

    public RobocopyProcess Unbuffered() => AddOption("J");

    public RobocopyProcess NoOffload() => AddOption("NOOFFLOAD");

    public RobocopyProcess NumberOfRetries(int numberOfRetries) =>
        AddOption(new WinArgument("/", "R", ":", numberOfRetries.ToString()));

    public RobocopyProcess WaitTimeBetweenRetries(int waitTimeInSeconds) =>
        AddOption(new WinArgument("/", "W", ":", waitTimeInSeconds.ToString()));

    public RobocopyProcess MultiThreaded(int numberOfThreads) =>
        AddOption(new WinArgument("/", "MT", ":", numberOfThreads.ToString()));

    public RobocopyProcess AddReadonlyAttributeToTarget()
    {
        attributesToAdd.Add("R");
        return this;
    }

    private RobocopyProcess AddOption(string name) => 
        AddOption(new WinArgument("/", name, "", ""));

    private RobocopyProcess AddOption(WinArgument arg)
    {
        options.Add(arg);
        return this;
    }

    private bool outputToConsole = false;

    public RobocopyProcess WriteOutputToConsole()
    {
        outputToConsole = true;
        return this;
    }

    public RobocopyProcess SetWorkingDirectory(string workingDirectory)
    {
        this.workingDirectory = workingDirectory;
        return this;
    }

    public string CommandText() => CreateProcess().CommandText();

    public async Task Run()
    {
        var process = CreateProcess();
        var result = await process.Run();
        if (result.ExitCode >= 8)
        {
            throw new Exception($"robocopy failed with exit code {result.ExitCode}");
        }
    }

    private WinProcess CreateProcess()
    {
        var process = new WinProcess("robocopy");
        if (outputToConsole)
        {
            process.WriteOutputToConsole();
        }
        process
            .UseArgumentNameDelimiter("")
            .AddArgument(new Quoted(source))
            .AddArgument(new Quoted(target))
            .AddArgument(pattern)
            .UseArgumentNameDelimiter("/")
            .UseArgumentValueDelimiter(":")
            .SetWorkingDirectory(workingDirectory);
        foreach (var arg in options)
        {
            process.AddArgument(arg);
        }
        if (attributesToAdd.Any())
        {
            process.AddArgument("a+", string.Join("", attributesToAdd));
        }
        return process;
    }
}