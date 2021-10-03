using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class YearlySchedule : IDaySchedule
    {
        private readonly YearDay[] yearDays;

        public YearlySchedule(params YearDay[] yearDays)
        {
            this.yearDays = yearDays;
        }

        public bool IsInRange(DateTimeOffset value)
            => yearDays.Any(d => d.ToDate(value) == value.Date);

        public DateTime[] AllowedDates(DateRange range)
            => range.Dates().Where(d => IsInRange(d)).ToArray();

    }
}
