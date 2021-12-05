namespace XTI_Schedule;

public sealed class ScheduleOptions
{
    private WeeklyScheduleOptions[] weeklySchedules = new WeeklyScheduleOptions[0];
    private MonthlyScheduleOptions[] monthlySchedules = new MonthlyScheduleOptions[0];
    private MonthlyOrdinalScheduleOptions[] monthlyOrdinalSchedules = new MonthlyOrdinalScheduleOptions[0];
    private YearlyScheduleOptions[] yearlySchedules = new YearlyScheduleOptions[0];
    private YearlyOrdinalScheduleOptions[] yearlyOrdinalSchedules = new YearlyOrdinalScheduleOptions[0];
    private PeriodicScheduleOptions[] periodicSchedules = new PeriodicScheduleOptions[0];

    public WeeklyScheduleOptions[] WeeklySchedules
    {
        get => weeklySchedules;
        set => weeklySchedules = value ?? new WeeklyScheduleOptions[0];
    }

    public MonthlyScheduleOptions[] MonthlySchedules
    {
        get => monthlySchedules;
        set => monthlySchedules = value ?? new MonthlyScheduleOptions[0];
    }

    public MonthlyOrdinalScheduleOptions[] MonthlyOrdinalSchedules
    {
        get => monthlyOrdinalSchedules;
        set => monthlyOrdinalSchedules = value ?? new MonthlyOrdinalScheduleOptions[0];
    }

    public YearlyScheduleOptions[] YearlySchedules
    {
        get => yearlySchedules;
        set => yearlySchedules = value ?? new YearlyScheduleOptions[0];
    }

    public YearlyOrdinalScheduleOptions[] YearlyOrdinalSchedules
    {
        get => yearlyOrdinalSchedules;
        set => yearlyOrdinalSchedules = value ?? new YearlyOrdinalScheduleOptions[0];
    }

    public PeriodicScheduleOptions[] PeriodicSchedules
    {
        get => periodicSchedules;
        set => periodicSchedules = value ?? new PeriodicScheduleOptions[0];
    }

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