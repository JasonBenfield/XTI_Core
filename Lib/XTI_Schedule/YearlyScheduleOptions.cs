using System.Linq;

namespace XTI_Schedule
{
    public sealed class YearlyScheduleOptions
    {
        public YearDay[] Days { get; set; } = new YearDay[] { };
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToSchedule()
            => new Schedule
            (
                new YearlySchedule(Days),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
