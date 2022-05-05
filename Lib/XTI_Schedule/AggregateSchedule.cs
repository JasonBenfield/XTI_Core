using XTI_Core;

namespace XTI_Schedule;

public sealed class AggregateSchedule
{
    private readonly Schedule[] schedules = new Schedule[0];

    public static AggregateSchedule Deserialize(string serialized) =>
        new AggregateSchedule
        (
            XtiSerializer.Deserialize<ScheduleOptions>(serialized).ToSchedules()
        );

    public AggregateSchedule(params Schedule[] schedules)
    {
        this.schedules = schedules ?? new Schedule[0];
    }

    public bool IsInSchedule(DateTimeOffset dateTime)
    {
        dateTime = dateTime.ToLocalTime();
        return schedules.Any(s => s.IsInSchedule(dateTime));
    }

    public DateTimeRange[] DateTimeRanges(DateRange dateRange)
        => schedules
            .SelectMany(s => s.DateTimeRanges(dateRange))
            .OrderBy(dr => dr)
            .Distinct()
            .ToArray();

    public string Serialize() => XtiSerializer.Serialize(ToScheduleOptions());

    public ScheduleOptions ToScheduleOptions()
    {
        var aggregateOptions = schedules.Select(s => s.ToScheduleOptions());
        return new ScheduleOptions
        {
            WeeklySchedules = aggregateOptions
                .SelectMany(s => s.WeeklySchedules)
                .ToArray(),
            MonthlySchedules = aggregateOptions
                .SelectMany(s => s.MonthlySchedules)
                .ToArray(),
            MonthlyOrdinalSchedules = aggregateOptions
                .SelectMany(s => s.MonthlyOrdinalSchedules)
                .ToArray(),
            YearlySchedules = aggregateOptions
                .SelectMany(s => s.YearlySchedules)
                .ToArray(),
            YearlyOrdinalSchedules = aggregateOptions
                .SelectMany(s => s.YearlyOrdinalSchedules)
                .ToArray(),
            PeriodicSchedules = aggregateOptions
                .SelectMany(s => s.PeriodicSchedules)
                .ToArray()
        };
    }
}