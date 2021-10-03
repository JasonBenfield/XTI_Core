using System;

namespace XTI_Schedule
{
    public sealed class YearlyOrdinalDayOfWeekOptions
    {
        public OrdinalWeek Week { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public Months[] Months { get; set; }

        public YearlyOrdinalDayOfWeek ToOrdinalDayOfWeek()
            => new YearlyOrdinalDayOfWeek(Week, DayOfWeek, Months);
    }
}
