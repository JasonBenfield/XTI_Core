using System.Linq;

namespace XTI_Schedule
{
    public sealed class ScheduleOptions
    {
        public WeeklyScheduleOptions[] WeeklySchedules { get; set; } = new WeeklyScheduleOptions[] { };

        public MonthlyScheduleOptions[] MonthlySchedules { get; set; } = new MonthlyScheduleOptions[] { };

        public MonthlyOrdinalScheduleOptions[] MonthlyOrdinalSchedules { get; set; } = new MonthlyOrdinalScheduleOptions[] { };

        public YearlyScheduleOptions[] YearlySchedules { get; set; } = new YearlyScheduleOptions[] { };

        public YearlyOrdinalScheduleOptions[] YearlyOrdinalSchedules { get; set; } = new YearlyOrdinalScheduleOptions[] { };

        public PeriodicScheduleOptions[] PeriodicSchedules { get; set; } = new PeriodicScheduleOptions[] { };

        public AggregateSchedule ToAggregateSchedule()
            => new AggregateSchedule(ToSchedules());

        public Schedule[] ToSchedules()
            => (WeeklySchedules ?? new WeeklyScheduleOptions[] { })
            .Select(tr => tr.ToSchedule())
            .Union
            (
                (MonthlySchedules ?? new MonthlyScheduleOptions[] { })
                    .Select(tr => tr.ToSchedule())
            )
            .Union
            (
                (MonthlyOrdinalSchedules ?? new MonthlyOrdinalScheduleOptions[] { })
                    .Select(tr => tr.ToSchedule())
            )
            .Union
            (
                (YearlySchedules ?? new YearlyScheduleOptions[] { })
                    .Select(tr => tr.ToSchedule())
            )
            .Union
            (
                (YearlyOrdinalSchedules ?? new YearlyOrdinalScheduleOptions[] { })
                    .Select(tr => tr.ToSchedule())
            )
            .Union
            (
                (PeriodicSchedules ?? new PeriodicScheduleOptions[] { })
                    .Select(tr => tr.ToSchedule())
            )
            .ToArray();
    }
}
