namespace XTI_Core;

public sealed class DbOptions
{
    public static readonly string DB = "DB";

    private string source = "";
    private string userName = "";
    private string password = "";
    private string qualifier = "";
    private string trustServerCertificate = "Yes";

    public string Source
    {
        get => source;
        set => source = value ?? "";
    }

    public string UserName
    {
        get => userName;
        set => userName = value ?? "";
    }

    public string Password
    {
        get => password;
        set => password = value ?? "";
    }

    public string Qualifier
    {
        get => qualifier;
        set => qualifier = value ?? "";
    }

    public string TrustServerCertificate
    {
        get => trustServerCertificate;
        set => trustServerCertificate = value ?? "";
    }

    public bool IsAlwaysEncryptedEnabled { get; set; }
}