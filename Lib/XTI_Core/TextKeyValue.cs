using System.Text.RegularExpressions;

namespace XTI_Core;

public partial class TextKeyValue : TextValue
{
    protected TextKeyValue(string displayText)
        : base(toTextValue(displayText))
    {
    }

    private static (string Value, string DisplayText) toTextValue(string displayText)
    {
        var normalizedDisplayText = normalizeDisplayText(displayText);
        var normalizedValue = normalizeValue(normalizedDisplayText);
        return (normalizedValue, normalizedDisplayText);
    }

    private static string normalizeDisplayText(string displayText)
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
        : base(normalizeValue(value), displayText)
    {
        if (string.IsNullOrWhiteSpace(displayText))
        {
            DisplayText = displayTextFromValue(Value);
        }
    }

    private static string normalizeValue(string value)
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

    private static string displayTextFromValue(string value) =>
        string.Join
        (
            ' ',
            value
                .Split('_')
                .Select(word => new CapitalizedWord(word).Value())
        );

    protected override bool _Equals(string? other) =>
        base._Equals(normalizeValue(other ?? ""));

    [GeneratedRegex("\\s|_+")]
    private static partial Regex SeparatorRegex();
}