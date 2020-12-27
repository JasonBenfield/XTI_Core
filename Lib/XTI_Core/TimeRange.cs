using System;

namespace XTI_Core
{
    public sealed class TimeRange : IEquatable<TimeRange>, IEquatable<DateRange>
    {
        public static TimeRange OnOrAfter(DateTimeOffset start)
        {
            return Between(start, DateTimeOffset.MaxValue);
        }

        public static TimeRange OnOrBefore(DateTimeOffset end)
        {
            return Between(DateTimeOffset.MinValue, end);
        }

        public static TimeRange Between(DateTimeOffset start, DateTimeOffset endTime)
        {
            return new TimeRange(start, endTime);
        }

        public TimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
            value = $"{Start}|{End}";
            hashCode = value.GetHashCode();
        }

        private readonly string value;
        private readonly int hashCode;

        public DateTimeOffset Start { get; }
        public DateTimeOffset End { get; }

        public bool IsInRange(DateTimeOffset value)
        {
            return value >= Start && value <= End;
        }

        public bool HasLowerBound() => Start > DateTimeOffset.MinValue;

        public bool HasUpperBound() => End < DateTimeOffset.MaxValue;

        public override string ToString() => $"{nameof(TimeRange)} {Format()}";

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
            if (obj is TimeRange tr)
            {
                return Equals(tr);
            }
            if (obj is DateRange dr)
            {
                return Equals(dr);
            }
            return base.Equals(obj);
        }

        public bool Equals(TimeRange other) => value == other?.value;

        public bool Equals(DateRange other) => other?.Equals(this) == true;

        public override int GetHashCode() => hashCode;

    }
}