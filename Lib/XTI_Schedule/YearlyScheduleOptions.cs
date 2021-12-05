namespace XTI_Schedule;

public sealed class YearlyScheduleOptions
{
    private YearDay[] days = new YearDay[0];
    private TimeRangeOptions[] timeRanges = new TimeRangeOptions[0];

    public YearDay[] Days
    {
        get => days;
        set => days = value ?? new YearDay[0];
    }

    public TimeRangeOptions[] TimeRanges
    {
        get => timeRanges;
        set => timeRanges = value ?? new TimeRangeOptions[0];
    }

    public Schedule ToSchedule()
        => new Schedule
        (
            new YearlySchedule(Days),
            (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
        );
}