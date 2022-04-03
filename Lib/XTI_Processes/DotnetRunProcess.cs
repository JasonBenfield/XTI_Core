namespace XTI_Processes;

public sealed class DotnetRunProcess : IProcess
{
    private readonly string project;
    private readonly OptionDictionary config = new OptionDictionary();
    private DotnetEnvironment environment = DotnetEnvironment.Development;
    private string workingDirectory = "";

    public DotnetRunProcess()
        : this("")
    {
    }

    public DotnetRunProcess(string project)
    {
        this.project = project;
    }

    public DotnetRunProcess UseEnvironment(string environmentName)
        => useEnvironment(DotnetEnvironment.FromValue(environmentName));

    public DotnetRunProcess UseDevelopmentEnvironment() => useEnvironment(DotnetEnvironment.Development);

    public DotnetRunProcess UseTestEnvironment() => useEnvironment(DotnetEnvironment.Test);

    public DotnetRunProcess UseStagingEnvironment() => useEnvironment(DotnetEnvironment.Staging);

    public DotnetRunProcess UseProductionEnvironment() => useEnvironment(DotnetEnvironment.Production);

    private DotnetRunProcess useEnvironment(DotnetEnvironment environment)
    {
        this.environment = environment;
        return this;
    }

    public DotnetRunProcess AddConfigOptions(object options, string name = "")
    {
        config.Add(options, name);
        return this;
    }

    public string CommandText()
    {
        var process = createProcess();
        return process.CommandText();
    }

    private bool outputToConsole = false;

    public DotnetRunProcess WriteOutputToConsole()
    {
        outputToConsole = true;
        return this;
    }

    public DotnetRunProcess SetWorkingDirectory(string workingDirectory)
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
        var process = new WinProcess("dotnet");
        process.SetWorkingDirectory(workingDirectory);
        if (outputToConsole)
        {
            process.WriteOutputToConsole();
        }
        process.UseArgumentNameDelimiter("");
        process.AddArgument("run");
        process.UseArgumentNameDelimiter("--");
        process.UseArgumentValueDelimiter(" ");
        process.AddArgument("project", new Quoted(project));
        process.AddArgument("environment", environment.Value);
        var dict = config.ToDictionary();
        foreach (var key in dict.Keys)
        {
            process.AddArgument(key, new Quoted(dict[key]));
        }
        return process;
    }
}