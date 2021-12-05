using XTI_Core;

namespace XTI_Schedule;

public sealed class TimeRangeOptions
{
    public Time Start { get; set; }
    public TimeSpan Duration { get; set; }

    public TimeRange ToTimeRange() => new TimeRange(Start, Duration);
}