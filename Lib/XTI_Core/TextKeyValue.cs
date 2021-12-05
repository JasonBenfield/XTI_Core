using System.Text.RegularExpressions;

namespace XTI_Core;

public class TextKeyValue : TextValue
{
    private static readonly Regex separatorRegex = new Regex("\\s|_+");

    protected TextKeyValue(string value)
        : base(normalizeValue(value), "")
    {
        DisplayText = displayTextFromValue(Value);
    }

    private static string normalizeValue(string value)
    {
        var normalized = value;
        if (separatorRegex.IsMatch(normalized))
        {
            normalized = separatorRegex.Replace(normalized.ToLower(), "_");
        }
        else
        {
            normalized = string.Join
            (
                '_',
                new CamelCasedWord(normalized).Words()
            )
            .ToLower();
        }
        return normalized;
    }

    private static string displayTextFromValue(string value)
        => string.Join
            (
                ' ',
                value
                    .Split('_')
                    .Select(word => new CapitalizedWord(word).Value())
            );

    protected override bool _Equals(string? other)
    {
        return base._Equals(normalizeValue(other ?? ""));
    }
}