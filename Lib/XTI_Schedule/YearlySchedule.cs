using System.Net.Http.Headers;
using XTI_Core;

namespace XTI_Schedule;

public sealed class YearlySchedule : IDaySchedule
{
    public YearlySchedule(params YearDay[] days)
    {
        Days = days;
    }

    internal YearDay[] Days { get; }

    public bool IsInRange(DateOnly value) =>
        Days.Any(d => d.WithYear(value.Year) == value);

    public DateOnly[] AllowedDates(DateRange range) =>
        range.Dates()
            .Where(d => IsInRange(d))
            .ToArray();

}