namespace XTI_Core;

public class TextValue : SemanticType<string>
{
    public TextValue(string value)
        : this(value, value)
    {
    }

    protected TextValue((string Value, string DisplayText) textValue)
        : this(textValue.Value, textValue.DisplayText)
    {
    }

    public TextValue(string value, string displayText)
        : base(value, displayText)
    {
    }

    protected bool _EqualsAny(params string[]? values) => values?.Any(v => Equals(v)) == true;
    protected bool _EqualsAny(params TextValue[]? values) => values?.Any(v => _Equals(v)) == true;
    protected override bool _Equals(string? other)
    {
        if (Value == null && other == null)
        {
            return true;
        }
        return Value?.Equals(other, StringComparison.OrdinalIgnoreCase) == true;
    }

    public override int GetHashCode() => base.GetHashCode();
    public override string ToString() => $"{GetType().Name} {Value}: {DisplayText}";
}