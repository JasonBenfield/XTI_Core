using System.Text.Json;

namespace XTI_Core;

public struct TimeRange
{
    public static TimeRange Deserialize(string str)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new TimeRangeJsonConverter());
        return JsonSerializer.Deserialize<TimeRange>(str, options);
    }

    public static Builder1 From(string time) => From(Time.Parse(time));

    public static Builder1 From(int hour, int minute = 0, int second = 0, int millisecond = 0) => From(new Time(hour, minute, second, millisecond));

    public static Builder1 From(Time time) => new Builder1(time);

    public static TimeRange AllDay() => From(new Time()).For(24).Hours();

    public TimeRange(Time startTime, TimeSpan duration)
    {
        Start = startTime;
        Duration = duration;
    }

    public Time Start { get; }
    public TimeSpan Duration { get; }

    public DateTimeOffset StartTime(DateTimeOffset date) => date.ToLocalTime().Date.Add(Start.ToTimeSpan());

    public DateTimeOffset EndTime(DateTimeOffset date) => StartTime(date).Add(Duration);

    public DateTimeRange Range(DateTimeOffset date)
        => DateTimeRange.Between(StartTime(date), EndTime(date));

    public bool IsInTimeRange(DateTimeOffset value)
        => DateTimeRange.Between(StartTime(value), EndTime(value)).IsInRange(value);

    public string Serialize()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new TimeRangeJsonConverter());
        return JsonSerializer.Serialize(this, options);
    }

    public override string ToString() => $"TimeRange({Start} for {Duration})";

    public sealed class Builder1
    {
        private readonly Time time;

        public Builder1(Time time)
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
        private readonly Time time;
        private readonly double quantity;

        public Builder2(Time time, double quantity)
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