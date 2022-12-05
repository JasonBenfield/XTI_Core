using System.Text.RegularExpressions;

namespace XTI_Core;

public sealed partial class CapitalizedWord
{
    private readonly string word;

    public CapitalizedWord(string word)
    {
        this.word = IgnoreRegex().IsMatch(word) ? word : word.ToLower();
    }

    public string Value()
    {
        string value;
        if (word.Length == 0)
        {
            value = "";
        }
        else if (word.Length == 1)
        {
            value = word.ToUpper();
        }
        else
        {
            value = word[0].ToString().ToUpper() + word.Substring(1);
        }
        return value;
    }

    public override string ToString() => $"{nameof(CapitalizedWord)} {word}";

    [GeneratedRegex("[A-Z]{2,}[a-z]+")]
    private static partial Regex IgnoreRegex();
}