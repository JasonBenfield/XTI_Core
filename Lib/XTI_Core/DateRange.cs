namespace XTI_Core;

public sealed class DateRange : IEquatable<DateRange>, IEquatable<DateTimeRange>, IComparable<DateRange>
{
    public static DateRange All() => Between(DateOnly.MinValue, DateOnly.MaxValue);

    public static DateRange OnOrAfter(DateTimeOffset start) => OnOrAfter(start.Date);

    public static DateRange OnOrAfter(DateTime start) => OnOrAfter(DateOnly.FromDateTime(start.Date));

    public static DateRange OnOrAfter(DateOnly start) => Between(start, DateOnly.MaxValue);

    public static DateRange OnOrBefore(DateTimeOffset end) => OnOrBefore(end.Date);

    public static DateRange OnOrBefore(DateTime end) => OnOrBefore(DateOnly.FromDateTime(end));

    public static DateRange OnOrBefore(DateOnly end) => Between(DateOnly.MinValue, end);

    public static DateRange On(DateTimeOffset date) => On(date.Date);

    public static DateRange On(DateTime date) => On(DateOnly.FromDateTime(date));

    public static DateRange On(DateOnly date) => Between(date, date);

    public static DateRange Between(DateTimeOffset start, DateTimeOffset end)
        => Between
        (
            start == DateTimeOffset.MinValue ? DateOnly.MinValue : DateOnly.FromDateTime(start.Date),
            end == DateTimeOffset.MaxValue ? DateOnly.MaxValue : DateOnly.FromDateTime(end.Date)
        );

    public static DateRange Between(DateTime start, DateTime end)
        => Between
        (
            start == DateTime.MinValue ? DateOnly.MinValue : DateOnly.FromDateTime(start.Date),
            end == DateTime.MaxValue ? DateOnly.MaxValue : DateOnly.FromDateTime(end.Date)
        );

    public static DateRange Between(DateOnly startDate, DateOnly endDate) =>
        new DateRange(startDate, endDate);

    public static Builder1 From(DateTimeOffset start) => From(start.LocalDateTime.Date);

    public static Builder1 From(DateTime start) => From(DateOnly.FromDateTime(start.Date));

    public static Builder1 From(DateOnly start) => new Builder1(start);

    private readonly int hashCode;

    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
        hashCode = $"{Start}|{End}".GetHashCode();
    }

    public DateOnly Start { get; }
    public DateOnly End { get; }

    public DateOnly[] Dates()
    {
        if (!HasLowerBound())
        {
            throw new ArgumentException("Unable to get dates when start is MinValue");
        }
        if (!HasUpperBound())
        {
            throw new ArgumentException("Unable to get dates when end is MaxValue");
        }
        var dates = new List<DateOnly>();
        var startDate = Start;
        var endDate = End;
        var date = startDate;
        while (date <= endDate)
        {
            dates.Add(date);
            date = date.AddDays(1);
        }
        return dates.ToArray();
    }

    public bool IsInRange(DateTimeOffset value) => IsInRange(value.Date);

    public bool IsInRange(DateTime value) => IsInRange(DateOnly.FromDateTime(value));

    public bool IsInRange(DateOnly value) => value >= Start && value <= End;

    public bool HasLowerBound() => Start > DateOnly.MinValue;

    public bool HasUpperBound() => End < DateOnly.MaxValue;

    public string Format()
    {
        string rangeText;
        if (HasLowerBound() && HasUpperBound())
        {
            if (Start == End)
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

    public override bool Equals(object? obj)
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

    public bool Equals(DateRange? other) =>
        Start == other?.Start && End == other?.End;

    public bool Equals(DateTimeRange? other)
    {
        bool equals;
        if (other == null)
        {
            equals = false;
        }
        else
        {
            equals = new DateTimeOffset(Start.ToDateTime(new TimeOnly())) == other.Start &&
                new DateTimeOffset(End.ToDateTime(new TimeOnly())) == other.End;
        }
        return equals;
    }

    public override int GetHashCode() => hashCode;

    public int CompareTo(DateRange? other)
    {
        int result = Start.CompareTo(other?.Start ?? DateOnly.MaxValue);
        if (result != 0)
        {
            result = End.CompareTo(other?.End ?? DateOnly.MaxValue);
        }
        return result;
    }

    public sealed class Builder1
    {
        private readonly DateOnly start;

        internal Builder1(DateOnly start)
        {
            this.start = start;
        }

        public DateRange Until(DateOnly end) => Between(start, end);

        public Builder2 For(int quantity) => new Builder2(start, quantity);

        public DateRange ForOneDay() => new Builder2(start, 1).Days();

        public DateRange ForOneWeek() => new Builder2(start, 1).Weeks();
    }

    public sealed class Builder2
    {
        private readonly DateOnly start;
        private readonly int quantity;

        public Builder2(DateOnly start, int quantity)
        {
            this.start = start;
            this.quantity = quantity;
        }

        public DateRange Days()
            => ToDateRange(TimeSpan.FromDays(quantity));

        public DateRange Weeks()
            => ToDateRange(TimeSpan.FromDays(quantity * 7));

        private DateRange ToDateRange(TimeSpan ts)
            => Between(start, start.AddDays(ts.Days));

    }
}