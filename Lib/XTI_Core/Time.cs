using System;
using System.ComponentModel;

namespace XTI_Core
{
    [TypeConverter(typeof(TimeTypeConverter))]
    public struct Time
    {
        private readonly DateTime date;

        public static Time Parse(string str)
        {
            var date = DateTime.Parse(str);
            return new Time(date);
        }

        public Time(DateTimeOffset value)
            : this(value.Hour, value.Minute, value.Second, value.Millisecond)
        {
        }

        public Time(DateTime value)
            : this(value.Hour, value.Minute, value.Second, value.Millisecond)
        {
        }

        public Time(int hour, int minute, int second = 0, int millisecond = 0)
        {
            date = DateTime.Today
                .AddHours(hour)
                .AddMinutes(minute)
                .AddSeconds(second)
                .AddMilliseconds(millisecond);
        }

        public int Hour { get => date.Hour; }
        public int Minute { get => date.Minute; }
        public int Second { get => date.Second; }
        public int Millisecond { get => date.Millisecond; }

        public DateTimeOffset ToDateTimeOffset(DateTimeOffset value)
            => value.Date
                .AddHours(Hour)
                .AddMinutes(Minute)
                .AddSeconds(Second)
                .AddMilliseconds(Millisecond);

        public DateTime ToDateTime(DateTime value)
            => value.Date
                .AddHours(Hour)
                .AddMinutes(Minute)
                .AddSeconds(Second)
                .AddMilliseconds(Millisecond);

        public TimeSpan ToTimeSpan() => new TimeSpan(0, Hour, Minute, Second, Millisecond);

        public override string ToString() => date.ToLongTimeString();

        public string ToString(string format) => date.ToString(format);
    }
}
