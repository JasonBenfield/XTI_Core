using System.Linq;

namespace XTI_Schedule
{
    public sealed class YearlyTimeRangeOptions
    {
        public YearDay[] Days { get; set; } = new YearDay[] { };
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToScheduleTimeRange()
            => new Schedule
            (
                new YearlySchedule(Days),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
