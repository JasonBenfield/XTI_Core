using System.Text.RegularExpressions;

namespace XTI_Core;

public sealed class CamelCasedWord : IEquatable<CamelCasedWord>, IEquatable<string>
{
    private readonly string word;
    private readonly bool isFirstWordLower = false;
    private static readonly Regex camelCasedRegex = new Regex("(([A-Z]{2,})(?!([a-z]+)))|([A-Z]{1}[a-z]+)|(\\d+)");

    public CamelCasedWord(string word)
    {
        this.word = word ?? "";
        if (this.word.Length > 0 && char.IsLower(this.word[0]))
        {
            var modified = char.ToUpper(this.word[0]).ToString();
            if (this.word.Length > 1)
            {
                modified += this.word.Substring(1);
            }
            this.word = modified;
            isFirstWordLower = true;
        }
    }

    public IEnumerable<string> Words()
        => camelCasedRegex
            .Matches(word)
            .SelectMany(m => m.Captures)
            .Select((c, i) => i == 0 && isFirstWordLower ? c.Value.ToLower() : c.Value);

    public override bool Equals(object? obj)
    {
        if (obj is CamelCasedWord ccw)
        {
            return Equals(ccw);
        }
        if (obj is string str)
        {
            return Equals(str);
        }
        return base.Equals(obj);
    }
    public bool Equals(CamelCasedWord? other) => Equals(other?.word);
    public bool Equals(string? other) => word == other;
    public override int GetHashCode() => word.GetHashCode();

    public override string ToString() => $"{nameof(CamelCasedWord)} {word}";
}