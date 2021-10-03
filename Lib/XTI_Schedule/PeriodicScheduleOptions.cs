using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTI_Schedule
{
    public sealed class PeriodicScheduleOptions
    {
        public int Frequency { get; set; }
        public DateInterval Interval { get; set; }
        public DateTime StartDate { get; set; }
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToScheduleTimeRange()
            => new Schedule
            (
                new PeriodicSchedule(Frequency, Interval, StartDate),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
