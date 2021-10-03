using System;

namespace XTI_Schedule
{
    public sealed class PeriodicScheduleBuilder
    {
        private readonly NextScheduleBuilder builder;
        private readonly int frequency;

        internal PeriodicScheduleBuilder(NextScheduleBuilder builder, int frequency)
        {
            this.builder = builder;
            this.frequency = frequency;
        }

        public NextPeriodicScheduleBuilder Days() => new NextPeriodicScheduleBuilder(builder, frequency, DateInterval.Days);
        public NextPeriodicScheduleBuilder Weeks() => new NextPeriodicScheduleBuilder(builder, frequency, DateInterval.Weeks);
        public NextPeriodicScheduleBuilder Months() => new NextPeriodicScheduleBuilder(builder, frequency, DateInterval.Months);
        public NextPeriodicScheduleBuilder Years() => new NextPeriodicScheduleBuilder(builder, frequency, DateInterval.Years);
    }

    public sealed class NextPeriodicScheduleBuilder
    {
        private readonly NextScheduleBuilder builder;
        private readonly int frequency;
        private readonly DateInterval interval;

        internal NextPeriodicScheduleBuilder(NextScheduleBuilder builder, int frequency, DateInterval interval)
        {
            this.builder = builder;
            this.frequency = frequency;
            this.interval = interval;
        }

        public NextScheduleBuilder Starting(DateTime when)
        {
            builder.Add(new PeriodicSchedule(frequency, interval, when));
            return builder;
        }
    }
}
