using System;
using System.Linq;

namespace XTI_Schedule
{
    public sealed class AggregateSchedule
    {
        private readonly Schedule[] schedules;

        public AggregateSchedule(params Schedule[] schedules)
        {
            this.schedules = schedules ?? new Schedule[] { };
        }

        public bool IsInSchedule(DateTimeOffset dateTime)
        {
            dateTime = dateTime.ToLocalTime();
            return schedules.Any(s => s.IsInSchedule(dateTime));
        }
    }
}
