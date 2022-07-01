using System.Text.RegularExpressions;

namespace XTI_Core;

public class TextKeyValue : TextValue
{
    private static readonly Regex separatorRegex = new Regex("\\s|_+");

    protected TextKeyValue(string displayText)
        : this
        (
            normalizeValue(displayText),
            string.Join
            (
                ' ',
                (
                    separatorRegex.IsMatch(displayText)
                        ? separatorRegex.Replace(displayText, " ")
                        : displayText
                )
                .Split(' ')
                .SelectMany
                (
                    w => new CamelCasedWord(w).Words()
                )
            )
        )
    {
    }

    protected TextKeyValue(string value, string displayText)
        : base(value, displayText)
    {
        if (string.IsNullOrWhiteSpace(displayText))
        {
            DisplayText = displayTextFromValue(Value);
        }
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

    protected override bool _Equals(string? other) =>
        base._Equals(normalizeValue(other ?? ""));

}