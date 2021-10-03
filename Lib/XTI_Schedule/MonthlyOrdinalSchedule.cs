using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class MonthlyOrdinalSchedule : IDaySchedule
    {
        private readonly MonthlyOrdinalDayOfWeek[] days;

        public MonthlyOrdinalSchedule(params MonthlyOrdinalDayOfWeek[] days)
        {
            this.days = days ?? new MonthlyOrdinalDayOfWeek[] { };
        }

        public bool IsInRange(DateTimeOffset value)
            => days.Any(d => d.ToDate(value.Date) == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range
                .Dates()
                .Where(d => IsInRange(d))
                .ToArray();
    }
}
