using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class YearlySchedule : IDaySchedule
    {
        public YearlySchedule(params YearDay[] days)
        {
            Days = days;
        }

        internal YearDay[] Days { get; }

        public bool IsInRange(DateTimeOffset value)
            => Days.Any(d => d.ToDate(value) == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range.Dates().Where(d => IsInRange(d)).ToArray();

    }
}
