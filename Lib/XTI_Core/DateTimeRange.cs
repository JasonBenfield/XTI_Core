using System;

namespace XTI_Core
{
    public sealed class DateTimeRange : IEquatable<DateTimeRange>, IEquatable<DateRange>, IComparable<DateTimeRange>
    {
        public static DateTimeRange OnOrAfter(DateTime start)
            => Between(start, DateTime.MaxValue);

        public static DateTimeRange OnOrAfter(DateTimeOffset start)
            => Between(start, DateTimeOffset.MaxValue);

        public static DateTimeRange OnOrBefore(DateTime end)
            => Between(DateTime.MinValue, end);

        public static DateTimeRange OnOrBefore(DateTimeOffset end)
            => Between(DateTimeOffset.MinValue, end);

        public static DateTimeRange Between(DateTime start, DateTime end)
            => Between
            (
                start > DateTime.MinValue ? new DateTimeOffset(start) : DateTimeOffset.MinValue,
                end < DateTime.MaxValue ? new DateTimeOffset(end) : DateTimeOffset.MaxValue
            );

        public static DateTimeRange Between(DateTimeOffset start, DateTimeOffset end)
            => new DateTimeRange(start, end);

        public static Builder1 From(DateTime start) => From(new DateTimeOffset(start));

        public static Builder1 From(DateTimeOffset start) => new Builder1(start);

        private readonly string value;
        private readonly int hashCode;

        public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
            value = $"{Start}|{End}";
            hashCode = value.GetHashCode();
        }

        public DateTimeOffset Start { get; }
        public DateTimeOffset End { get; }

        public bool IsInRange(DateTimeOffset value)
            => value >= Start && value <= End;

        public bool HasLowerBound() => Start > DateTimeOffset.MinValue;

        public bool HasUpperBound() => End < DateTimeOffset.MaxValue;

        public override string ToString() => $"{nameof(DateTimeRange)} {Format()}";

        public string Format()
        {
            string rangeText;
            if (HasLowerBound() && HasUpperBound())
            {
                rangeText = $"{Start.LocalDateTime:M/dd/yy h:mm tt} to {End.LocalDateTime:M/dd/yy h:mm tt}";
            }
            else if (HasLowerBound())
            {
                rangeText = $"On or After {Start.LocalDateTime:M/dd/yy h:mm tt}";
            }
            else if (HasUpperBound())
            {
                rangeText = $"On or Before {End.LocalDateTime:M/dd/yy h:mm tt}";
            }
            else
            {
                rangeText = "";
            }
            return rangeText;
        }

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

        public bool Equals(DateTimeRange other) => value == other?.value;

        public bool Equals(DateRange other) => other?.Equals(this) == true;

        public override int GetHashCode() => hashCode;

        public int CompareTo(DateTimeRange other)
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
            private readonly DateTimeOffset start;

            internal Builder1(DateTimeOffset start)
            {
                this.start = start;
            }

            public DateTimeRange Until(DateTimeOffset end) => DateTimeRange.Between(start, end);

            public Builder2 For(double quantity) => new Builder2(start, quantity);

            public DateTimeRange ForOneMillisecond() => new Builder2(start, 1).Milliseconds();

            public DateTimeRange ForOneSecond() => new Builder2(start, 1).Seconds();

            public DateTimeRange ForOneMinute() => new Builder2(start, 1).Minutes();

            public DateTimeRange ForOneHour() => new Builder2(start, 1).Hours();

            public DateTimeRange ForOneDay() => new Builder2(start, 1).Days();

            public DateTimeRange ForOneWeek() => new Builder2(start, 1).Weeks();
        }

        public sealed class Builder2
        {
            private readonly DateTimeOffset start;
            private readonly double quantity;

            public Builder2(DateTimeOffset start, double quantity)
            {
                this.start = start;
                this.quantity = quantity;
            }

            public DateTimeRange Milliseconds()
                => ToDateTimeRange(TimeSpan.FromMilliseconds(quantity));

            public DateTimeRange Seconds()
                => ToDateTimeRange(TimeSpan.FromSeconds(quantity));

            public DateTimeRange Minutes()
                => ToDateTimeRange(TimeSpan.FromMinutes(quantity));

            public DateTimeRange Hours()
                => ToDateTimeRange(TimeSpan.FromHours(quantity));

            public DateTimeRange Days()
                => ToDateTimeRange(TimeSpan.FromDays(quantity));

            public DateTimeRange Weeks()
                => ToDateTimeRange(TimeSpan.FromDays(quantity * 7));

            private DateTimeRange ToDateTimeRange(TimeSpan ts)
                => Between(start, start.Add(ts));

        }
    }
}