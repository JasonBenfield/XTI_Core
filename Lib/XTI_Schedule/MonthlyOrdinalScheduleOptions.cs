using System.Linq;

namespace XTI_Schedule
{
    public sealed class MonthlyOrdinalScheduleOptions
    {
        public MonthlyOrdinalDayOfWeekOptions[] Days { get; set; } = new MonthlyOrdinalDayOfWeekOptions[] { };
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToScheduleTimeRange()
            => new Schedule
            (
                new MonthlyOrdinalSchedule
                (
                    (Days ?? new MonthlyOrdinalDayOfWeekOptions[] { })
                        .Select(d => d.ToOrdinalDayOfWeek())
                        .ToArray()
                ),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
