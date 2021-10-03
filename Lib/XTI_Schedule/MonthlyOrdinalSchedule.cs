using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class MonthlyOrdinalSchedule : IDaySchedule
    {
        public MonthlyOrdinalSchedule(params MonthlyOrdinalDayOfWeek[] days)
        {
            Days = days ?? new MonthlyOrdinalDayOfWeek[] { };
        }

        internal MonthlyOrdinalDayOfWeek[] Days { get; }

        public bool IsInRange(DateTimeOffset value)
            => Days.Any(d => d.ToDate(value.Date) == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range
                .Dates()
                .Where(d => IsInRange(d))
                .ToArray();
    }
}
