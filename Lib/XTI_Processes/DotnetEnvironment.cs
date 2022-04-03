namespace XTI_Processes;

internal sealed class DotnetEnvironment
{
    public static readonly DotnetEnvironment Development = new DotnetEnvironment(nameof(Development));
    public static readonly DotnetEnvironment Test = new DotnetEnvironment(nameof(Test));
    public static readonly DotnetEnvironment Staging = new DotnetEnvironment(nameof(Staging));
    public static readonly DotnetEnvironment Production = new DotnetEnvironment(nameof(Production));

    private static readonly DotnetEnvironment[] environments = new[] { Development, Test, Staging, Production };

    public static DotnetEnvironment FromValue(string value)
    {
        var env = environments.FirstOrDefault(e => e.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
        if (env == null)
        {
            throw new NotSupportedException($"Environment '{value}' is not supported");
        }
        return env;
    }

    public DotnetEnvironment(string environment)
    {
        Value = environment;
    }

    public string Value { get; }
}