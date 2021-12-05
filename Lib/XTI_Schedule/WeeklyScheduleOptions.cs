namespace XTI_Schedule;

public sealed class WeeklyScheduleOptions
{
    private DayOfWeek[] days = new DayOfWeek[0];
    private TimeRangeOptions[] timeRanges = new TimeRangeOptions[0];

    public DayOfWeek[] Days
    {
        get => days;
        set => days = value ?? new DayOfWeek[0];
    }

    public TimeRangeOptions[] TimeRanges
    {
        get => timeRanges;
        set => timeRanges = value ?? new TimeRangeOptions[0];
    }

    public Schedule ToSchedule()
        => new Schedule
        (
            new WeeklySchedule(Days),
            (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
        );
}