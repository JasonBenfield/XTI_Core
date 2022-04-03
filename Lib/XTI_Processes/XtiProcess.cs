namespace XTI_Processes;

public sealed class XtiProcess : IProcess
{
    private readonly string fileName;
    private readonly OptionDictionary config = new OptionDictionary();
    private DotnetEnvironment environment = DotnetEnvironment.Development;

    private bool outputToConsole = false;

    private string workingDirectory = "";

    public XtiProcess(string fileName)
    {
        this.fileName = fileName;
    }

    public XtiProcess UseEnvironment(string environmentName)
        => useEnvironment(DotnetEnvironment.FromValue(environmentName));

    public XtiProcess UseDevelopmentEnvironment() => useEnvironment(DotnetEnvironment.Development);

    public XtiProcess UseTestEnvironment() => useEnvironment(DotnetEnvironment.Test);

    public XtiProcess UseStagingEnvironment() => useEnvironment(DotnetEnvironment.Staging);

    public XtiProcess UseProductionEnvironment() => useEnvironment(DotnetEnvironment.Production);

    private XtiProcess useEnvironment(DotnetEnvironment environment)
    {
        this.environment = environment;
        return this;
    }

    public XtiProcess AddConfigOptions(object options, string name = "")
    {
        config.Add(options, name);
        return this;
    }

    public async Task<WinProcessResult> Run()
    {
        var process = createWinProcess();
        var result = await process.Run();
        return result;
    }

    public string CommandText()
    {
        var process = createWinProcess();
        return process.CommandText();
    }

    public XtiProcess WriteOutputToConsole()
    {
        outputToConsole = true;
        return this;
    }

    public XtiProcess SetWorkingDirectory(string workingDirectory)
    {
        this.workingDirectory = workingDirectory;
        return this;
    }

    private WinProcess createWinProcess()
    {
        var process = new WinProcess(fileName);
        if (outputToConsole)
        {
            process.WriteOutputToConsole();
        }
        process.UseArgumentNameDelimiter("--");
        process.UseArgumentValueDelimiter(" ");
        process.AddArgument("environment", environment.Value);
        process.SetWorkingDirectory(workingDirectory);
        var dict = config.ToDictionary();
        foreach (var key in dict.Keys)
        {
            process.AddArgument(key, new Quoted(dict[key]));
        }
        return process;
    }
}