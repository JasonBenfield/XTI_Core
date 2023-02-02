namespace XTI_Schedule;

public sealed class PeriodicScheduleOptions
{
    private TimeRangeOptions[] timeRanges = new TimeRangeOptions[0];

    public int Frequency { get; set; }
    public DateInterval Interval { get; set; }
    public DateOnly StartDate { get; set; }
    public TimeRangeOptions[] TimeRanges
    {
        get => timeRanges;
        set => timeRanges = value ?? new TimeRangeOptions[0];
    }

    public Schedule ToSchedule()
        => new Schedule
        (
            new PeriodicSchedule(Frequency, Interval, StartDate),
            (TimeRanges ?? new TimeRangeOptions[0]).Select(tr => tr.ToTimeRange()).ToArray()
        );
}