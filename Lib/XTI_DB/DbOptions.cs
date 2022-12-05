namespace XTI_DB;

public sealed class DbOptions
{
    public static readonly string DB = "DB";

    private string source = "";
    private string userName = "";
    private string password = "";
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

    public string TrustServerCertificate
    {
        get => trustServerCertificate;
        set => trustServerCertificate = value ?? "";
    }
}