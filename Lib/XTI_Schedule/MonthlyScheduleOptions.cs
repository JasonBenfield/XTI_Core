using System.Linq;

namespace XTI_Schedule
{
    public sealed class MonthlyScheduleOptions
    {
        public MonthDay[] Days { get; set; } = new MonthDay[] { };
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToSchedule()
            => new Schedule
            (
                new MonthlySchedule(Days),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
