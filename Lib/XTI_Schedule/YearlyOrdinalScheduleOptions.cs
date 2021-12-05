namespace XTI_Schedule;

public sealed class YearlyOrdinalScheduleOptions
{
    private YearlyOrdinalDayOfWeekOptions[] days = new YearlyOrdinalDayOfWeekOptions[0];
    private TimeRangeOptions[] timeRanges = new TimeRangeOptions[0];

    public YearlyOrdinalDayOfWeekOptions[] Days
    {
        get => days;
        set => days = value ?? new YearlyOrdinalDayOfWeekOptions[0];
    }

    public TimeRangeOptions[] TimeRanges
    {
        get => timeRanges;
        set => timeRanges = value ?? new TimeRangeOptions[0];
    }

    public Schedule ToSchedule()
        => new Schedule
        (
            new YearlyOrdinalSchedule
            (
                (Days ?? new YearlyOrdinalDayOfWeekOptions[] { })
                    .Select(d => d.ToOrdinalDayOfWeek())
                    .ToArray()
            ),
            TimeRanges.Select(tr => tr.ToTimeRange()).ToArray()
        );
}