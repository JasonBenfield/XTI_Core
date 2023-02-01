using XTI_Core;

namespace XTI_Schedule;

public sealed class YearlyOrdinalSchedule : IDaySchedule
{
    public YearlyOrdinalSchedule(params YearlyOrdinalDayOfWeek[] days)
    {
        Days = days;
    }

    internal YearlyOrdinalDayOfWeek[] Days { get; }

    public bool IsInRange(DateOnly value)
        => Days
            .SelectMany(d => d.ToDates(value))
            .Any(d => d == value);

    public DateOnly[] AllowedDates(DateRange range)
        => range
            .Dates()
            .Where(d => IsInRange(d))
            .ToArray();
}