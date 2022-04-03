namespace XTI_Core;

public sealed class XtiEnvironmentAccessor
{
    public static XtiEnvironmentAccessor Development() => new XtiEnvironmentAccessor(XtiEnvironment.Development);
    public static XtiEnvironmentAccessor Test() => new XtiEnvironmentAccessor(XtiEnvironment.Test);
    public static XtiEnvironmentAccessor Staging() => new XtiEnvironmentAccessor(XtiEnvironment.Staging);
    public static XtiEnvironmentAccessor Production() => new XtiEnvironmentAccessor(XtiEnvironment.Production);
    public static XtiEnvironmentAccessor Shared() => new XtiEnvironmentAccessor(XtiEnvironment.Shared);

    public XtiEnvironmentAccessor()
        : this(XtiEnvironment.Development)
    {
    }

    public XtiEnvironmentAccessor(XtiEnvironment environment)
    {
        Environment = environment;
    }

    public XtiEnvironment Environment { get; private set; }

    public void UseEnvironment(string envName) => Environment = XtiEnvironment.Parse(envName);
    public void UseDevelopment() => Environment = XtiEnvironment.Development;
    public void UseTest() => Environment = XtiEnvironment.Test;
    public void UseStaging() => Environment = XtiEnvironment.Staging;
    public void UseProduction() => Environment = XtiEnvironment.Production;
    public void UseShared() => Environment = XtiEnvironment.Shared;

    public override string ToString() => $"{nameof(XtiEnvironmentAccessor)} {Environment.EnvironmentName}";
}
