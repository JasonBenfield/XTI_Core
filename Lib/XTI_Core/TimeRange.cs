using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core;

[JsonConverter(typeof(TimeRangeJsonConverter))]
public struct TimeRange
{
    public static Builder1 From(string time) => From(TimeOnly.Parse(time));

    public static Builder1 From(int hour, int minute = 0, int second = 0, int millisecond = 0) => From(new TimeOnly(hour, minute, second, millisecond));

    public static Builder1 From(TimeOnly time) => new Builder1(time);

    public static TimeRange AllDay() => From(new TimeOnly()).For(24).Hours();

    public TimeRange(TimeOnly startTime, TimeSpan duration)
    {
        Start = startTime;
        Duration = duration;
    }

    public TimeOnly Start { get; }
    public TimeSpan Duration { get; }

    public DateTimeOffset StartTime(DateOnly date) => new DateTimeOffset(date.ToDateTime(Start));

    public DateTimeOffset EndTime(DateOnly date) => StartTime(date).Add(Duration);

    public DateTimeRange Range(DateOnly date)
        => DateTimeRange.Between(StartTime(date), EndTime(date));

    public bool IsInTimeRange(TimeOnly time)
        => IsInTimeRange(DateOnly.FromDateTime(DateTime.Today).ToDateTime(time));

    public bool IsInTimeRange(DateTime dateTime)
        => IsInTimeRange(new DateTimeOffset(dateTime));

    public bool IsInTimeRange(DateTimeOffset value)
        => DateTimeRange.Between
        (
            StartTime(DateOnly.FromDateTime(value.Date)), 
            EndTime(DateOnly.FromDateTime(value.Date))
        )
        .IsInRange(value);

    public override string ToString() => $"TimeRange({Start} for {Duration})";

    public sealed class Builder1
    {
        private readonly TimeOnly time;

        public Builder1(TimeOnly time)
        {
            this.time = time;
        }

        public Builder2 For(double quantity) => new Builder2(time, quantity);

        public TimeRange ForOneMillisecond() => new Builder2(time, 1).Milliseconds();

        public TimeRange ForOneSecond() => new Builder2(time, 1).Seconds();

        public TimeRange ForOneMinute() => new Builder2(time, 1).Minutes();

        public TimeRange ForOneHour() => new Builder2(time, 1).Hours();
    }

    public sealed class Builder2
    {
        private readonly TimeOnly time;
        private readonly double quantity;

        public Builder2(TimeOnly time, double quantity)
        {
            this.time = time;
            this.quantity = quantity;
        }

        public TimeRange Milliseconds()
            => new TimeRange(time, TimeSpan.FromMilliseconds(quantity));

        public TimeRange Seconds()
            => new TimeRange(time, TimeSpan.FromSeconds(quantity));

        public TimeRange Minutes()
            => new TimeRange(time, TimeSpan.FromMinutes(quantity));

        public TimeRange Hours()
            => new TimeRange(time, TimeSpan.FromHours(quantity));

    }
}