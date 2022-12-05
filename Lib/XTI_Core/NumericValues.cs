using System.Text.RegularExpressions;

namespace XTI_Core;

public partial class NumericValues<T> where T : NumericValue
{
    private readonly List<T> values = new ();

    protected NumericValues(T defaultValue)
    {
        DefaultValue = defaultValue;
        Add(DefaultValue);
    }

    protected T DefaultValue { get; }

    protected T Add(T value)
    {
        values.Add(value);
        return value;
    }

    public T Value(int value) =>
        values.FirstOrDefault(nv => nv.Equals(value)) ?? DefaultValue;

    public T Value(string displayText)
    {
        if (OnlyDigitsRegex().IsMatch(displayText))
        {
            return Value(int.Parse(displayText));
        }
        return values
            .FirstOrDefault
            (
                v => WhitespaceRegex().Replace(v.DisplayText, "")
                    .Equals(WhitespaceRegex().Replace(displayText, ""), StringComparison.OrdinalIgnoreCase)
            ) ?? DefaultValue;
    }

    public T GetDefault() => DefaultValue;

    public T[] GetAll() => values.ToArray();

    [GeneratedRegex("\\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex("^\\d+$")]
    private static partial Regex OnlyDigitsRegex();
}