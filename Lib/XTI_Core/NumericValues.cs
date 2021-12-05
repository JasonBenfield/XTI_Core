using System.Text.RegularExpressions;

namespace XTI_Core;

public class NumericValues<T> where T : NumericValue
{
    private static readonly Regex whitespaceRegex = new Regex("\\s+");
    private static readonly Regex onlyDigitsRegex = new Regex("^\\d+$");

    private readonly List<T> values = new List<T>();

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
        if (onlyDigitsRegex.IsMatch(displayText))
        {
            return Value(int.Parse(displayText));
        }
        return values
            .FirstOrDefault
            (
                v => whitespaceRegex.Replace(v.DisplayText, "")
                    .Equals(whitespaceRegex.Replace(displayText, ""), StringComparison.OrdinalIgnoreCase)
            ) ?? DefaultValue;
    }

    public T[] All() => values.ToArray();
}