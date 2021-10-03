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

        public static Builder On(params DayOfWeek[] daysOfWeek)
            => On(new WeeklySchedule(daysOfWeek));

        public static Builder EveryDay() => On(WeeklySchedule.EveryDay());

        public static Builder Weekdays() => On(WeeklySchedule.Weekdays());

        public static Builder Weekends() => On(WeeklySchedule.Weekend());

        public static Builder On(params MonthDay[] days) => On(new MonthlySchedule(days));

        public static Builder On(params YearDay[] days) => On(new YearlySchedule(days));

        public static OrdinalBuilder First(DayOfWeek day) => new OrdinalBuilder(new Builder(), OrdinalWeek.First, day);

        public static OrdinalBuilder Second(DayOfWeek day) => new OrdinalBuilder(new Builder(), OrdinalWeek.Second, day);

        public static OrdinalBuilder Third(DayOfWeek day) => new OrdinalBuilder(new Builder(), OrdinalWeek.Third, day);

        public static OrdinalBuilder Fourth(DayOfWeek day) => new OrdinalBuilder(new Builder(), OrdinalWeek.Fourth, day);

        public static OrdinalBuilder Last(DayOfWeek day) => new OrdinalBuilder(new Builder(), OrdinalWeek.Last, day);

        public static PeriodicBuilder2 EveryWeek() => new PeriodicBuilder1(new Builder(), 1).Weeks();

        public static PeriodicBuilder2 EveryMonth() => new PeriodicBuilder1(new Builder(), 1).Months();

        public static PeriodicBuilder2 EveryYear() => new PeriodicBuilder1(new Builder(), 1).Years();

        public static PeriodicBuilder1 Every(int frequency) => new PeriodicBuilder1(new Builder(), frequency);

        private static Builder On(IDaySchedule daySchedule) => new Builder(daySchedule);

        public Schedule(IDaySchedule daySchedule, params TimeRange[] timeRanges)
            : this(new[] { daySchedule }, timeRanges)
        {
        }

        public Schedule(IDaySchedule[] daySchedules, params TimeRange[] timeRanges)
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

        public sealed class Builder
        {
            private readonly List<IDaySchedule> daySchedules = new List<IDaySchedule>();

            internal Builder(IDaySchedule daySchedule = null)
            {
                if (daySchedule != null)
                {
                    daySchedules.Add(daySchedule);
                }
            }

            public Builder AndOn(params DayOfWeek[] daysOfWeek)
                => On(new WeeklySchedule(daysOfWeek));

            public Builder AndEveryDay() => AndOn(WeeklySchedule.EveryDay());

            public Builder AndWeekdays() => AndOn(WeeklySchedule.Weekdays());

            public Builder AndWeekends() => AndOn(WeeklySchedule.Weekend());

            public Builder AndOn(params MonthDay[] days) => AndOn(new MonthlySchedule(days));

            public OrdinalBuilder AndTheFirst(DayOfWeek day) => new OrdinalBuilder(this, OrdinalWeek.First, day);

            public OrdinalBuilder AndTheSecond(DayOfWeek day) => new OrdinalBuilder(this, OrdinalWeek.Second, day);

            public OrdinalBuilder AndTheThird(DayOfWeek day) => new OrdinalBuilder(this, OrdinalWeek.Third, day);

            public OrdinalBuilder AndTheFourth(DayOfWeek day) => new OrdinalBuilder(this, OrdinalWeek.Fourth, day);

            public OrdinalBuilder AndTheLast(DayOfWeek day) => new OrdinalBuilder(this, OrdinalWeek.Last, day);

            public PeriodicBuilder2 AndEveryWeek() => new PeriodicBuilder1(this, 1).Weeks();

            public PeriodicBuilder2 AndEveryMonth() => new PeriodicBuilder1(this, 1).Months();

            public PeriodicBuilder2 AndEveryYear() => new PeriodicBuilder1(this, 1).Years();

            public PeriodicBuilder1 AndEvery(int frequency) => new PeriodicBuilder1(this, frequency);

            private Builder AndOn(IDaySchedule daySchedule)
            {
                daySchedules.Add(daySchedule);
                return this;
            }

            internal Builder Add(IDaySchedule daySchedule)
            {
                daySchedules.Add(daySchedule);
                return this;
            }

            public Schedule At(params TimeRange[] timeRanges)
                => new Schedule(daySchedules.ToArray(), timeRanges);
        }

        public sealed class OrdinalBuilder
        {
            private readonly Builder builder;
            private readonly OrdinalWeek week;
            private readonly DayOfWeek day;

            internal OrdinalBuilder(Builder builder, OrdinalWeek week, DayOfWeek day)
            {
                this.builder = builder;
                this.week = week;
                this.day = day;
            }

            public Builder OfEveryMonth()
            {
                builder.Add(new MonthlyOrdinalSchedule(new MonthlyOrdinalDayOfWeek(week, day)));
                return builder;
            }

            public Builder Of(params Months[] months)
            {
                builder.Add(new YearlyOrdinalSchedule(new YearlyOrdinalDayOfWeek(week, day, months)));
                return builder;
            }
        }

        public sealed class PeriodicBuilder1
        {
            private readonly Builder builder;
            private readonly int frequency;

            internal PeriodicBuilder1(Builder builder, int frequency)
            {
                this.builder = builder;
                this.frequency = frequency;
            }

            public PeriodicBuilder2 Days() => new PeriodicBuilder2(builder, frequency, DateInterval.Days);
            public PeriodicBuilder2 Weeks() => new PeriodicBuilder2(builder, frequency, DateInterval.Weeks);
            public PeriodicBuilder2 Months() => new PeriodicBuilder2(builder, frequency, DateInterval.Months);
            public PeriodicBuilder2 Years() => new PeriodicBuilder2(builder, frequency, DateInterval.Years);
        }

        public sealed class PeriodicBuilder2
        {
            private readonly Builder builder;
            private readonly int frequency;
            private readonly DateInterval interval;

            internal PeriodicBuilder2(Builder builder, int frequency, DateInterval interval)
            {
                this.builder = builder;
                this.frequency = frequency;
                this.interval = interval;
            }

            public Builder Starting(DateTime when)
            {
                builder.Add(new PeriodicSchedule(frequency, interval, when));
                return builder;
            }
        }
    }


}
