using XTI_Core;

namespace XTI_DB;

public sealed class XtiConnectionString
{
    private readonly string value;

    public XtiConnectionString(DbOptions options, XtiDbName dbName)
    {
        var connStr = new Dictionary<string, string>();
        connStr.Add("Data Source", options.Source);
        if (string.IsNullOrWhiteSpace(options.UserName))
        {
            connStr.Add("Trusted_Connection", "True");
        }
        else
        {
            connStr.Add("User Id", options.UserName);
            connStr.Add("Password", options.Password);
        }
        connStr.Add("Initial Catalog", dbName.Value);
        if (!string.IsNullOrWhiteSpace(options.TrustServerCertificate))
        {
            connStr.Add("TrustServerCertificate", "Yes");
        }
        if (options.IsAlwaysEncryptedEnabled)
        {
            connStr.Add("Column Encryption Setting", "enabled");
        }
        value = string.Join(";", connStr.Keys.Select(key => $"{key}={connStr[key]}"));
    }

    public string Value() => value;

    public override string ToString() => $"{GetType().Name} {value}";
}