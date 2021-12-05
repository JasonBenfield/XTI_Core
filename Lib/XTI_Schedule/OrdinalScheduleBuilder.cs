namespace XTI_Schedule;

public sealed class OrdinalScheduleBuilder
{
    private readonly NextScheduleBuilder builder;
    private readonly OrdinalWeek week;
    private readonly DayOfWeek day;

    internal OrdinalScheduleBuilder(NextScheduleBuilder builder, OrdinalWeek week, DayOfWeek day)
    {
        this.builder = builder;
        this.week = week;
        this.day = day;
    }

    public NextScheduleBuilder OfEveryMonth()
    {
        builder.Add(new MonthlyOrdinalSchedule(new MonthlyOrdinalDayOfWeek(week, day)));
        return builder;
    }

    public NextScheduleBuilder Of(params Months[] months)
    {
        builder.Add(new YearlyOrdinalSchedule(new YearlyOrdinalDayOfWeek(week, day, months)));
        return builder;
    }
}
