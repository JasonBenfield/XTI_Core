using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class YearlyOrdinalSchedule : IDaySchedule
    {
        private readonly YearlyOrdinalDayOfWeek[] days;

        public YearlyOrdinalSchedule(params YearlyOrdinalDayOfWeek[] days)
        {
            this.days = days;
        }

        public bool IsInRange(DateTimeOffset value)
            => days
                .SelectMany(d => d.ToDates(value.Date))
                .Any(d => d == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range
                .Dates()
                .Where(d => IsInRange(d))
                .ToArray();
    }
}
