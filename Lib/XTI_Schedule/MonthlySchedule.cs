using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class MonthlySchedule : IDaySchedule
    {
        public MonthlySchedule(params MonthDay[] days)
        {
            Days = days ?? new MonthDay[] { };
        }

        internal MonthDay[] Days { get; }

        public bool IsInRange(DateTimeOffset value)
            => Days.Any(d => d.ToDate(value) == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range
                .Dates()
                .Where(d => IsInRange(d))
                .ToArray();
    }
}
