﻿namespace XTI_DB;

public sealed class XtiDbName
{
    public XtiDbName(string environmentName, string dbName)
    {
        Value = $"XTI_{environmentName}_{dbName}";
    }

    public string Value { get; }

    public override string ToString() => $"{nameof(XtiDbName)} {Value}";
}