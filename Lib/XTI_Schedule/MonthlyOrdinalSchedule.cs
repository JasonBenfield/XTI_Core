using XTI_Core;

namespace XTI_Schedule;

public sealed class MonthlyOrdinalSchedule : IDaySchedule
{
    public MonthlyOrdinalSchedule(params MonthlyOrdinalDayOfWeek[] days)
    {
        Days = days ?? new MonthlyOrdinalDayOfWeek[0];
    }

    internal MonthlyOrdinalDayOfWeek[] Days { get; }

    public bool IsInRange(DateOnly value)
        => Days.Any(d => d.ToDate(value) == value);

    public DateOnly[] AllowedDates(DateRange range)
        => range
            .Dates()
            .Where(d => IsInRange(d))
            .ToArray();
}