namespace XTI_Core;

public sealed class XtiEnvironment : IEquatable<XtiEnvironment>, IEquatable<string>
{
    public static readonly XtiEnvironment Development = new XtiEnvironment(nameof(Development));
    public static readonly XtiEnvironment Test = new XtiEnvironment(nameof(Test));
    public static readonly XtiEnvironment Staging = new XtiEnvironment(nameof(Staging));
    public static readonly XtiEnvironment Production = new XtiEnvironment(nameof(Production));
    public static readonly XtiEnvironment Shared = new XtiEnvironment(nameof(Shared));

    private static readonly XtiEnvironment[] All = new[] { Development, Test, Staging, Production, Shared };

    public static XtiEnvironment Parse(string envName) =>
        All.FirstOrDefault(env => env.Equals(envName)) ?? Development;

    public XtiEnvironment(string environmentName)
    {
        EnvironmentName = environmentName;
    }

    public string EnvironmentName { get; }

    public bool IsDevelopmentOrTest() => IsDevelopment() || IsTest();
    public bool IsDevelopment() => Equals(Development);
    public bool IsTest() => Equals(Test);
    public bool IsStaging() => Equals(Staging);
    public bool IsProduction() => Equals(Production);
    public bool IsShared() => Equals(Shared);

    public override int GetHashCode() => EnvironmentName.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null) { return false; }
        if (obj is XtiEnvironment env) { return Equals(env); }
        if (obj is string str) { return Equals(str); }
        return base.Equals(obj);
    }

    public bool Equals(XtiEnvironment? other) => Equals(other?.EnvironmentName);

    public bool Equals(string? other) => EnvironmentName.Equals(other, StringComparison.OrdinalIgnoreCase);

    public override string ToString() => $"{nameof(XtiEnvironment)} {EnvironmentName}";
}
