﻿using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace XTI_Schedule;

[TypeConverter(typeof(MonthDayTypeConverter))]
[JsonConverter(typeof(MonthDayJsonConverter))]
public partial struct MonthDay
{
    public static readonly MonthDay LastDay = new(int.MaxValue);

    public static MonthDay Parse(string str)
    {
        MonthDay monthDay;
        var match = regex().Match(str);
        if (match.Groups["Last"].Success)
        {
            monthDay = LastDay;
        }
        else if (match.Groups["Day"].Success)
        {
            monthDay = new MonthDay(int.Parse(match.Groups["Day"].Value));
        }
        else
        {
            monthDay = new MonthDay();
        }
        return monthDay;
    }

    public MonthDay(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public bool IsLastDay() => Value == int.MaxValue;

    public DateOnly ToDate(DateOnly date)
    {
        if (IsLastDay())
        {
            return new DateOnly(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }
        return new DateOnly(date.Year, date.Month, 1).AddDays(Value - 1);
    }

    public string Format()
        => IsLastDay() ? "Last" : Value.ToString();

    public override string ToString() => $"{nameof(MonthDay)} {Format()}";

    [GeneratedRegex("^(?:(?<Last>Last)|(?<Day>\\d{1,2}))$", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex regex();
}
