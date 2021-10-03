using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class WeeklySchedule : IDaySchedule
    {
        private readonly DayOfWeek[] daysOfWeek;

        public static WeeklySchedule EveryDay()
            => new WeeklySchedule
            (
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            );

        public static WeeklySchedule Weekdays()
            => new WeeklySchedule
            (
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            );

        public static WeeklySchedule Weekend()
            => new WeeklySchedule
            (
                DayOfWeek.Sunday,
                DayOfWeek.Saturday
            );

        public WeeklySchedule(params DayOfWeek[] daysOfWeek)
        {
            this.daysOfWeek = daysOfWeek;
        }

        public bool IsInRange(DateTimeOffset value)
        {
            value = value.ToLocalTime();
            return daysOfWeek.Any(d => value.DayOfWeek == d);
        }

        public DateTime[] AllowedDates(DateRange range)
            => range.Dates()
                .Where(d => IsInRange(d))
                .ToArray();
    }
}
