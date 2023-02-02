using XTI_Core;

namespace XTI_Schedule;

public sealed class MonthlySchedule : IDaySchedule
{
    public MonthlySchedule(params MonthDay[] days)
    {
        Days = days ?? new MonthDay[0];
    }

    internal MonthDay[] Days { get; }

    public bool IsInRange(DateOnly value)
        => Days.Any(d => d.ToDate(value) == value);

    public DateOnly[] AllowedDates(DateRange range)
        => range
            .Dates()
            .Where(d => IsInRange(d))
            .ToArray();
}