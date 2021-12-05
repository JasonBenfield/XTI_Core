using XTI_Core;

namespace XTI_Schedule;

public sealed class YearlyOrdinalSchedule : IDaySchedule
{
    public YearlyOrdinalSchedule(params YearlyOrdinalDayOfWeek[] days)
    {
        Days = days;
    }

    internal YearlyOrdinalDayOfWeek[] Days { get; }

    public bool IsInRange(DateTimeOffset value)
        => Days
            .SelectMany(d => d.ToDates(value.Date))
            .Any(d => d == value.Date);

    public DateTime[] AllowedDates(DateRange range)
        => range
            .Dates()
            .Where(d => IsInRange(d))
            .ToArray();
}