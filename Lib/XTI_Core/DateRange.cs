using System;
using System.Collections.Generic;

namespace XTI_Core
{
    public sealed class DateRange : IEquatable<DateRange>, IEquatable<DateTimeRange>, IComparable<DateRange>
    {
        public static DateRange OnOrAfter(DateTimeOffset start) => OnOrAfter(start.Date);

        public static DateRange OnOrAfter(DateTime start) => Between(start, DateTime.MaxValue);

        public static DateRange OnOrBefore(DateTimeOffset end) => OnOrBefore(end.Date);

        public static DateRange OnOrBefore(DateTime end) => Between(DateTime.MinValue, end);

        public static DateRange On(DateTimeOffset date) => On(date.Date);

        public static DateRange On(DateTime date) => Between(date, date);

        public static DateRange Between(DateTimeOffset start, DateTimeOffset end)
            => Between(start.Date, end.Date);

        public static DateRange Between(DateTime startDate, DateTime endDate)
        {
            if (startDate > DateTime.MinValue)
            {
                startDate = startDate.Date;
            }
            if (endDate < DateTime.MaxValue)
            {
                endDate = endDate.Date.AddDays(1).AddTicks(-1);
            }
            return new DateRange(DateTimeRange.Between(startDate, endDate));
        }

        public static Builder1 From(DateTimeOffset start) => From(start.LocalDateTime.Date);

        public static Builder1 From(DateTime start) => new Builder1(start);

        private DateRange(DateTimeRange timeRange)
        {
            TimeRange = timeRange;
            Start = timeRange.Start.Date;
            End = timeRange.End.Date;
        }

        public DateTimeRange TimeRange { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public DateTime[] Dates()
        {
            if (TimeRange.Start == DateTimeOffset.MinValue)
            {
                throw new ArgumentException("Unable to get dates when start is MinValue");
            }
            if (TimeRange.End == DateTimeOffset.MaxValue)
            {
                throw new ArgumentException("Unable to get dates when end is MaxValue");
            }
            var dates = new List<DateTime>();
            var startDate = TimeRange.Start.Date;
            var endDate = TimeRange.End.Date;
            var date = startDate;
            while (date <= endDate)
            {
                dates.Add(date);
                date = date.AddDays(1);
            }
            return dates.ToArray();
        }

        public bool IsInRange(DateTimeOffset value) => IsInRange(value.Date);

        public bool IsInRange(DateTime value) => TimeRange.IsInRange(value);

        public bool HasLowerBound() => TimeRange.HasLowerBound();

        public bool HasUpperBound() => TimeRange.HasUpperBound();

        public string Format()
        {
            string rangeText;
            if (HasLowerBound() && HasUpperBound())
            {
                if (Start.Date == End.Date)
                {
                    rangeText = $"On {Start:M/dd/yy}";
                }
                else
                {
                    rangeText = $"From {Start:M/dd/yy} to {End:M/dd/yy}";
                }
            }
            else if (HasLowerBound())
            {
                rangeText = $"On or After {Start:M/dd/yy}";
            }
            else if (HasUpperBound())
            {
                rangeText = $"On or Before {End:M/dd/yy}";
            }
            else
            {
                rangeText = "";
            }
            return rangeText;
        }

        public override string ToString() => $"{nameof(DateRange)} {Format()}";

        public override bool Equals(object obj)
        {
            if (obj is DateTimeRange tr)
            {
                return Equals(tr);
            }
            if (obj is DateRange dr)
            {
                return Equals(dr);
            }
            return base.Equals(obj);
        }

        public bool Equals(DateRange other) => TimeRange.Equals(other?.TimeRange);

        public bool Equals(DateTimeRange other) => TimeRange.Equals(other);

        public override int GetHashCode() => TimeRange.GetHashCode();

        public int CompareTo(DateRange other)
        {
            int result = Start.CompareTo(other?.Start ?? DateTimeOffset.MaxValue);
            if (result != 0)
            {
                result = End.CompareTo(other?.End ?? DateTimeOffset.MaxValue);
            }
            return result;
        }

        public sealed class Builder1
        {
            private readonly DateTime start;

            internal Builder1(DateTime start)
            {
                this.start = start;
            }

            public DateRange Until(DateTimeOffset end) => Between(start, end);

            public Builder2 For(double quantity) => new Builder2(start, quantity);

            public DateRange ForOneMillisecond() => new Builder2(start, 1).Milliseconds();

            public DateRange ForOneSecond() => new Builder2(start, 1).Seconds();

            public DateRange ForOneMinute() => new Builder2(start, 1).Minutes();

            public DateRange ForOneHour() => new Builder2(start, 1).Hours();

            public DateRange ForOneDay() => new Builder2(start, 1).Days();

            public DateRange ForOneWeek() => new Builder2(start, 1).Weeks();
        }

        public sealed class Builder2
        {
            private readonly DateTime start;
            private readonly double quantity;

            public Builder2(DateTime start, double quantity)
            {
                this.start = start;
                this.quantity = quantity;
            }

            public DateRange Milliseconds()
                => ToDateRange(TimeSpan.FromMilliseconds(quantity));

            public DateRange Seconds()
                => ToDateRange(TimeSpan.FromSeconds(quantity));

            public DateRange Minutes()
                => ToDateRange(TimeSpan.FromMinutes(quantity));

            public DateRange Hours()
                => ToDateRange(TimeSpan.FromHours(quantity));

            public DateRange Days()
                => ToDateRange(TimeSpan.FromDays(quantity));

            public DateRange Weeks()
                => ToDateRange(TimeSpan.FromDays(quantity * 7));

            private DateRange ToDateRange(TimeSpan ts)
                => Between(start, start.Add(ts));

        }
    }
}