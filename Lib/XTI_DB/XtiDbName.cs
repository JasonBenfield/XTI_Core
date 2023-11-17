namespace XTI_DB;

public sealed class XtiDbName
{
    public XtiDbName(string environmentName, string dbName)
        : this(environmentName, dbName, "")
    {
    }

    public XtiDbName(string environmentName, string dbName, string qualifier)
    {
        var qualifierValue = string.IsNullOrWhiteSpace(qualifier) ? "" : $"_{qualifier}";
        Value = $"XTI_{environmentName}{qualifierValue}_{dbName}";
    }

    public string Value { get; }

    public override string ToString() => $"{nameof(XtiDbName)} {Value}";
}