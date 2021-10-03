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
    }
}