using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class MonthlySchedule : IDaySchedule
    {
        private readonly MonthDay[] days;

        public MonthlySchedule(params MonthDay[] days)
        {
            this.days = days ?? new MonthDay[] { };
        }

        public bool IsInRange(DateTimeOffset value)
            => days.Any(d => d.ToDate(value) == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range
                .Dates()
                .Where(d => IsInRange(d))
                .ToArray();
    }
}
