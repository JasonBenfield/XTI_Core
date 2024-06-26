﻿namespace XTI_Core;

public abstract class NumericValue : SemanticType<int>, IEquatable<int>
{
    private static readonly Func<int, string, string> defaultFormatDisplayText =
        (int value, string displayText) => new CamelCasedWord(displayText).Format();

    protected static readonly Func<int, string, string> NoFormat =
        (int value, string displayText) => displayText.Trim();

    protected NumericValue(int value, string displayText)
        : this(value, displayText, defaultFormatDisplayText)
    {
    }

    protected NumericValue(int value, string displayText, Func<int, string, string> formatDisplayText)
        : base(value, formatDisplayText(value, displayText))
    {
    }

    protected bool _EqualsAny(params int[] values) => values.Any(v => Equals(v));
    protected bool _EqualsAny(params NumericValue[] values) => values.Any(v => _Equals(v));

    public override string ToString() => $"{GetType().Name} {Value}: {DisplayText}";
}