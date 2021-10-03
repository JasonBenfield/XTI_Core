using System;

namespace XTI_Schedule
{
    public sealed class MonthlyOrdinalDayOfWeekOptions
    {
        public OrdinalWeek Week { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public MonthlyOrdinalDayOfWeek ToOrdinalDayOfWeek() => new MonthlyOrdinalDayOfWeek(Week, DayOfWeek);
    }
}
