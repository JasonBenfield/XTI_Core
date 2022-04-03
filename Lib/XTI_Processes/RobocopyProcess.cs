﻿namespace XTI_Processes;

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

    public RobocopyProcess CopySubdirectoriesIncludingEmpty() => addOption("e");

    public RobocopyProcess Purge() => addOption("purge");

    public RobocopyProcess NoJobHeader() => addOption("njh");

    public RobocopyProcess NoJobSummary() => addOption("njs");

    public RobocopyProcess NoDirectoryLogging() => addOption("ndl");

    public RobocopyProcess NoFileLogging() => addOption("nfl");

    public RobocopyProcess NoFileSizeLogging() => addOption("ns");

    public RobocopyProcess NoFileClassLogging() => addOption("nc");

    public RobocopyProcess NoProgressDisplayed() => addOption("np");

    public RobocopyProcess MoveFiles() => addOption("mov");

    public RobocopyProcess MoveFilesAndDirectories() => addOption("move");

    public RobocopyProcess AddReadonlyAttributeToTarget()
    {
        attributesToAdd.Add("R");
        return this;
    }

    private RobocopyProcess addOption(string name)
        => addOption(new WinArgument("/", name, "", ""));

    private RobocopyProcess addOption(WinArgument arg)
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

    public async Task Run()
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
        var result = await process.Run();
        if (result.ExitCode >= 8)
        {
            throw new Exception($"robocopy failed with exit code {result.ExitCode}");
        }
    }
}