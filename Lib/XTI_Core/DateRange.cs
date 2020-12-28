using System;

namespace XTI_Core
{
    public sealed class DateRange : IEquatable<DateRange>, IEquatable<TimeRange>
    {
        public static DateRange OnOrAfter(DateTimeOffset start)
        {
            return Between(start, DateTimeOffset.MaxValue);
        }

        public static DateRange OnOrBefore(DateTimeOffset end)
        {
            return Between(DateTimeOffset.MinValue, end);
        }

        public static DateRange On(DateTimeOffset date)
        {
            return Between(date, date);
        }

        public static DateRange Between(DateTimeOffset start, DateTimeOffset end)
        {
            if (start > DateTimeOffset.MinValue)
            {
                start = start.Date;
            }
            if (end < DateTimeOffset.MaxValue)
            {
                end = end.Date.AddDays(1).AddTicks(-1);
            }
            return new DateRange(TimeRange.Between(start, end));
        }


        private DateRange(TimeRange timeRange)
        {
            TimeRange = timeRange;
        }

        public TimeRange TimeRange { get; }
        public DateTimeOffset Start { get => TimeRange.Start; }
        public DateTimeOffset End { get => TimeRange.End; }

        public bool IsInRange(DateTimeOffset value) => TimeRange.IsInRange(value);

        public bool HasLowerBound() => TimeRange.HasLowerBound();

        public bool HasUpperBound() => TimeRange.HasUpperBound();

        public string Format()
        {
            string rangeText;
            if (HasLowerBound() && HasUpperBound())
            {
                if (Start.Date == End.Date)
                {
                    rangeText = $"On {Start.LocalDateTime:M/dd/yy}";
                }
                else
                {
                    rangeText = $"From {Start.LocalDateTime:M/dd/yy} to {End.LocalDateTime:M/dd/yy}";
                }
            }
            else if (HasLowerBound())
            {
                rangeText = $"On or After {Start.LocalDateTime:M/dd/yy}";
            }
            else if (HasUpperBound())
            {
                rangeText = $"On or Before {End.LocalDateTime:M/dd/yy}";
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

        public bool Equals(DateRange other) => TimeRange.Equals(other?.TimeRange);

        public bool Equals(TimeRange other) => TimeRange.Equals(other);

        public override int GetHashCode() => TimeRange.GetHashCode();
    }
}