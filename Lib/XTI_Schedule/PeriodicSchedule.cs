using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class PeriodicSchedule : IDaySchedule
    {
        public static Builder2 EveryDay() => new Builder1(1).Days();

        public static Builder2 EveryWeek() => new Builder1(1).Weeks();

        public static Builder2 EveryMonth() => new Builder1(1).Months();

        public static Builder2 EveryYear() => new Builder1(1).Years();

        public static Builder1 Every(int frequency) => new Builder1(frequency);

        public PeriodicSchedule(int frequency, DateInterval interval, DateTimeOffset startDate)
            : this(frequency, interval, startDate.Date)
        {
        }

        public PeriodicSchedule(int frequency, DateInterval interval, DateTime startDate)
        {
            Frequency = frequency;
            Interval = interval;
            StartDate = startDate.Date;
        }

        internal int Frequency { get; }
        internal DateInterval Interval { get; }
        internal DateTime StartDate { get; }

        public bool IsInRange(DateTimeOffset value)
        {
            var dateValue = value.Date;
            if (dateValue < StartDate)
            {
                return false;
            }
            if (StartDate == dateValue)
            {
                return true;
            }
            else if (Interval == DateInterval.Days || Interval == DateInterval.Weeks)
            {
                var numberOfDays = Frequency;
                if (Interval == DateInterval.Weeks)
                {
                    numberOfDays = Frequency * 7;
                }
                var ts = dateValue - StartDate;
                return ts.TotalDays % numberOfDays == 0;
            }
            else if (Interval == DateInterval.Months)
            {
                var date = StartDate.Date;
                while (date < dateValue)
                {
                    date = date.AddMonths(Frequency);
                }
                return date == dateValue;
            }
            else if (Interval == DateInterval.Years)
            {
                var date = StartDate.Date;
                while (date < dateValue)
                {
                    date = date.AddYears(Frequency);
                }
                return date == dateValue;
            }
            return false;
        }

        public DateTime[] AllowedDates(DateRange range)
            => range
                .Dates()
                .Where(d => IsInRange(d))
                .ToArray();

        public sealed class Builder1
        {
            private readonly int frequency;

            internal Builder1(int frequency)
            {
                this.frequency = frequency;
            }

            public Builder2 Days() => new Builder2(frequency, DateInterval.Days);
            public Builder2 Weeks() => new Builder2(frequency, DateInterval.Weeks);
            public Builder2 Months() => new Builder2(frequency, DateInterval.Months);
            public Builder2 Years() => new Builder2(frequency, DateInterval.Years);
        }

        public sealed class Builder2
        {
            private readonly int frequency;
            private readonly DateInterval interval;

            internal Builder2(int frequency, DateInterval interval)
            {
                this.frequency = frequency;
                this.interval = interval;
            }

            public PeriodicSchedule Starting(DateTime when)
                => new PeriodicSchedule(frequency, interval, when);
        }
    }
}
