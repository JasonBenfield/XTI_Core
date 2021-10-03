using System;
using System.Collections.Generic;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class Schedule
    {
        private readonly IDaySchedule[] daySchedules;
        private readonly TimeRange[] timeRanges;

        public static ScheduleBuilder Build() => new ScheduleBuilder();

        public static NextScheduleBuilder On(params DayOfWeek[] daysOfWeek)
            => Build().On(daysOfWeek);

        public static NextScheduleBuilder EveryDay() => Build().EveryDay();

        public static NextScheduleBuilder Weekdays() => Build().Weekdays();

        public static NextScheduleBuilder Weekends() => Build().Weekends();

        public static NextScheduleBuilder On(params MonthDay[] days) => Build().On(days);

        public static NextScheduleBuilder On(params YearDay[] days) => Build().On(days);

        public static OrdinalScheduleBuilder First(DayOfWeek day) => Build().First(day);

        public static OrdinalScheduleBuilder Second(DayOfWeek day) => Build().Second(day);

        public static OrdinalScheduleBuilder Third(DayOfWeek day) => Build().Third(day);

        public static OrdinalScheduleBuilder Fourth(DayOfWeek day) => Build().Fourth(day);

        public static OrdinalScheduleBuilder Last(DayOfWeek day) => Build().Last(day);

        public static NextPeriodicScheduleBuilder EveryWeek() => Build().EveryWeek();

        public static NextPeriodicScheduleBuilder EveryMonth() => Build().EveryMonth();

        public static NextPeriodicScheduleBuilder EveryYear() => Build().EveryYear();

        public static PeriodicScheduleBuilder Every(int frequency) => Build().Every(frequency);

        internal Schedule(IDaySchedule daySchedule, params TimeRange[] timeRanges)
            : this(new[] { daySchedule }, timeRanges)
        {
        }

        internal Schedule(IDaySchedule[] daySchedules, params TimeRange[] timeRanges)
        {
            this.daySchedules = daySchedules ?? new IDaySchedule[] { };
            this.timeRanges = timeRanges ?? new TimeRange[] { };
        }

        public bool IsInSchedule(DateTime value) => IsInSchedule(new DateTimeOffset(value));

        public bool IsInSchedule(DateTimeOffset value)
        {
            value = value.ToLocalTime();
            return daySchedules.Any(dr => dr.IsInRange(value))
                && timeRanges.Any(tr => tr.IsInTimeRange(value));
        }

        public DateTimeRange[] DateTimeRanges(DateRange range)
            => daySchedules
                .SelectMany(dr => dr.AllowedDates(range))
                .SelectMany(d => timeRanges.Select(tr => tr.Range(d)))
                .OrderBy(tr => tr)
                .ToArray();

        public ScheduleOptions ToScheduleOptions()
        {
            var timeRangeOptions = timeRanges
                .Select
                (
                    tr => new TimeRangeOptions
                    {
                        Start = tr.Start,
                        Duration = tr.Duration
                    }
                )
                .ToArray();
            var weeklySchedules = daySchedules.OfType<WeeklySchedule>()
                .Select
                (
                    w => new WeeklyScheduleOptions
                    {
                        Days = w.Days,
                        TimeRanges = timeRangeOptions
                    }
                )
                .ToArray();
            var monthlySchedules = daySchedules.OfType<MonthlySchedule>()
                .Select
                (
                    m => new MonthlyScheduleOptions
                    {
                        Days = m.Days,
                        TimeRanges = timeRangeOptions
                    }
                )
                .ToArray();
            var monthlyOrdinalSchedules = daySchedules.OfType<MonthlyOrdinalSchedule>()
                .Select
                (
                    m => new MonthlyOrdinalScheduleOptions
                    {
                        Days = m.Days
                            .Select
                            (
                                d => new MonthlyOrdinalDayOfWeekOptions
                                {
                                    Week = d.Week,
                                    DayOfWeek = d.DayOfWeek
                                }
                            )
                            .ToArray(),
                        TimeRanges = timeRangeOptions
                    }
                )
                .ToArray();
            var yearlySchedules = daySchedules.OfType<YearlySchedule>()
                .Select
                (
                    y => new YearlyScheduleOptions
                    {
                        Days = y.Days,
                        TimeRanges = timeRangeOptions
                    }
                )
                .ToArray();
            var yearlyOrdinalSchedules = daySchedules.OfType<YearlyOrdinalSchedule>()
                .Select
                (
                    y => new YearlyOrdinalScheduleOptions
                    {
                        Days = y.Days
                            .Select
                            (
                                d => new YearlyOrdinalDayOfWeekOptions
                                {
                                    Week = d.Week,
                                    DayOfWeek = d.DayOfWeek,
                                    Months = d.Months
                                }
                            )
                            .ToArray(),
                        TimeRanges = timeRangeOptions
                    }
                )
                .ToArray();
            var periodicSchedules = daySchedules.OfType<PeriodicSchedule>()
                .Select
                (
                    y => new PeriodicScheduleOptions
                    {
                        Frequency = y.Frequency,
                        Interval = y.Interval,
                        StartDate = y.StartDate,
                        TimeRanges = timeRangeOptions
                    }
                )
                .ToArray();
            return new ScheduleOptions()
            {
                WeeklySchedules = weeklySchedules,
                MonthlySchedules = monthlySchedules,
                MonthlyOrdinalSchedules = monthlyOrdinalSchedules,
                YearlySchedules = yearlySchedules,
                YearlyOrdinalSchedules = yearlyOrdinalSchedules,
                PeriodicSchedules = periodicSchedules
            };
        }
    }


}
