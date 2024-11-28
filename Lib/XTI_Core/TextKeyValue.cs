using System.Text.RegularExpressions;

namespace XTI_Core;

public partial class TextKeyValue : TextValue
{
    protected TextKeyValue(string displayText)
        : base(ToTextValue(displayText))
    {
    }

    private static (string Value, string DisplayText) ToTextValue(string displayText)
    {
        var normalizedDisplayText = NormalizeDisplayText(displayText);
        var normalizedValue = NormalizeValue(normalizedDisplayText);
        return (normalizedValue, normalizedDisplayText);
    }

    private static string NormalizeDisplayText(string displayText)
    {
        var source =
            SeparatorRegex().IsMatch(displayText)
                ? SeparatorRegex().Replace(displayText, " ")
                : displayText;
        var normalized = string.Join
        (
            ' ',
            source
                .Split(' ')
                .SelectMany
                (
                    w => new CamelCasedWord(w).Words()
                )
        );
        return normalized;
    }

    protected TextKeyValue(string value, string displayText)
        : base(NormalizeValue(value), displayText)
    {
        if (string.IsNullOrWhiteSpace(displayText))
        {
            DisplayText = DisplayTextFromValue(Value);
        }
    }

    private static string NormalizeValue(string value)
    {
        var normalized = value;
        if (SeparatorRegex().IsMatch(normalized))
        {
            normalized = SeparatorRegex().Replace(normalized.ToLower(), "_");
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

    private static string DisplayTextFromValue(string value) =>
        string.Join
        (
            ' ',
            value
                .Split('_')
                .Select(word => new CapitalizedWord(word).Value())
        );

    protected override bool _Equals(string? other) =>
        base._Equals(NormalizeValue(other ?? ""));

    [GeneratedRegex("\\s|_+")]
    private static partial Regex SeparatorRegex();
}