namespace XTI_Core;

public sealed class TruncatedText
{
    public TruncatedText(string value, int maxLength)
    {
        OriginalValue = value;
        Value = value.Length > maxLength ? value.Substring(0, maxLength) : value;
    }

    public string OriginalValue { get; }
    public string Value { get; }
}
