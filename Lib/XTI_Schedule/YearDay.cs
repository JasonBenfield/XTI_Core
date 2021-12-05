using System.ComponentModel;
using System.Text.RegularExpressions;

namespace XTI_Schedule;

[TypeConverter(typeof(YearDayTypeConverter))]
public struct YearDay
{
    private static readonly Regex regex = new Regex("^\\s*(?<Month>\\d{1,2})[\\/-](?<Day>\\d{1,2})\\s*$");

    public static YearDay Parse(string str)
    {
        var match = regex.Match(str);
        if (match.Success)
        {
            return new YearDay
            (
                int.Parse(match.Groups["Month"].Value),
                int.Parse(match.Groups["Day"].Value)
            );
        }
        return new YearDay();
    }

    public YearDay(int month, int day)
    {
        Month = month;
        Day = day;
    }

    public int Month { get; }
    public int Day { get; }

    public DateTime ToDate(DateTimeOffset value) => new DateTime(value.Year, Month, Day);

    public string Format() => $"{Month}/{Day}";

    public override string ToString() => $"{nameof(YearDay)} {Format()}";
}