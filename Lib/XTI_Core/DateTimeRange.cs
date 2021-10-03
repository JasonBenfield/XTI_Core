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
        {
            return new DateTimeRange(start, end);
        }

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
    }
}