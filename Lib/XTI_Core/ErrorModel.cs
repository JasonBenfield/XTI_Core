namespace XTI_Core;

public sealed record ErrorModel(string Message, string Caption, string Source)
{
    public ErrorModel()
        : this("")
    {
    }

    public ErrorModel(string message) : this(message, "", "")
    {
    }
}