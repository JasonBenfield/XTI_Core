namespace XTI_Processes;

internal sealed class WinArgument
{
    private readonly string name;
    private readonly string value;
    private readonly string nameDelimiter;
    private readonly string valueDelimiter;
    private string? output;

    public WinArgument(string nameDelimiter, string name, string valueDelimiter, string value)
    {
        this.nameDelimiter = nameDelimiter;
        this.name = name;
        this.valueDelimiter = valueDelimiter;
        this.value = value;
    }

    public string Output()
    {
        if (output == null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                output = $"{nameDelimiter}{name}".Trim();
            }
            else
            {
                output = $"{nameDelimiter}{name}{valueDelimiter}{value}".Trim();
            }
        }
        return output;
    }

    public override string ToString() => $"{nameof(WinArgument)} {Output()}";
}