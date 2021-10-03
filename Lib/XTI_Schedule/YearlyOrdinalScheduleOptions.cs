using System.Linq;

namespace XTI_Schedule
{
    public sealed class YearlyOrdinalScheduleOptions
    {
        public YearlyOrdinalDayOfWeekOptions[] Days { get; set; } = new YearlyOrdinalDayOfWeekOptions[] { };
        public TimeRangeOptions[] TimeRanges { get; set; } = new TimeRangeOptions[] { };

        public Schedule ToScheduleTimeRange()
            => new Schedule
            (
                new YearlyOrdinalSchedule
                (
                    (Days ?? new YearlyOrdinalDayOfWeekOptions[] { })
                        .Select(d => d.ToOrdinalDayOfWeek())
                        .ToArray()
                ),
                (TimeRanges ?? new TimeRangeOptions[] { }).Select(tr => tr.ToTimeRange()).ToArray()
            );
    }
}
