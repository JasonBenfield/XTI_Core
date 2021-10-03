using System;
using System.Linq;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class PeriodicSchedule : IDaySchedule
    {
        private readonly int frequency;
        private readonly DateInterval interval;
        private readonly DateTime startDate;

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
            this.frequency = frequency;
            this.interval = interval;
            this.startDate = startDate.Date;
        }

        public bool IsInRange(DateTimeOffset value)
        {
            var dateValue = value.Date;
            if (dateValue < startDate)
            {
                return false;
            }
            if (startDate == dateValue)
            {
                return true;
            }
            else if (interval == DateInterval.Days || interval == DateInterval.Weeks)
            {
                var numberOfDays = frequency;
                if (interval == DateInterval.Weeks)
                {
                    numberOfDays = frequency * 7;
                }
                var ts = dateValue - startDate;
                return ts.TotalDays % numberOfDays == 0;
            }
            else if (interval == DateInterval.Months)
            {
                var date = startDate.Date;
                while (date < dateValue)
                {
                    date = date.AddMonths(frequency);
                }
                return date == dateValue;
            }
            else if (interval == DateInterval.Years)
            {
                var date = startDate.Date;
                while (date < dateValue)
                {
                    date = date.AddYears(frequency);
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
