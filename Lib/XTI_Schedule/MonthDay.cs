using System.ComponentModel;
using System.Text.RegularExpressions;

namespace XTI_Schedule;

[TypeConverter(typeof(MonthDayTypeConverter))]
public struct MonthDay
{
    public static readonly MonthDay LastDay = new MonthDay(int.MaxValue);

    private readonly static Regex regex = new Regex("^(?:(?<Last>Last)|(?<Day>\\d{1,2}))$", RegexOptions.IgnoreCase);

    public static MonthDay Parse(string str)
    {
        MonthDay monthDay;
        var match = regex.Match(str);
        if (match.Groups["Last"].Success)
        {
            monthDay = MonthDay.LastDay;
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

    public DateTime ToDate(DateTimeOffset time)
    {
        if (IsLastDay())
        {
            return new DateTime(time.Year, time.Month, 1).AddMonths(1).AddDays(-1);
        }
        return new DateTime(time.Year, time.Month, 1).AddDays(Value - 1);
    }

    public string Format()
        => IsLastDay() ? "Last" : Value.ToString();

    public override string ToString() => $"{nameof(MonthDay)} {Format()}";
}
