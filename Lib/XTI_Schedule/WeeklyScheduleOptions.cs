using System;
using System.Linq;

namespace XTI_Schedule
{
    public sealed class WeeklyScheduleOptions
    {
        public DayOfWeek[] Days { get; set; } = new DayOfWeek[] { };
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToSchedule()
            => new Schedule
            (
                new WeeklySchedule(Days),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
