namespace XTI_Schedule;

public struct YearlyOrdinalDayOfWeek
{
    private readonly MonthlyOrdinalDayOfWeek ordinal;

    public YearlyOrdinalDayOfWeek(OrdinalWeek week, DayOfWeek day, params Months[] months)
    {
        ordinal = new MonthlyOrdinalDayOfWeek(week, day);
        Months = months ?? new Months[0];
    }

    public OrdinalWeek Week { get => ordinal.Week; }
    public DayOfWeek DayOfWeek { get => ordinal.DayOfWeek; }
    public Months[] Months { get; }

    public DateOnly[] ToDates(DateOnly dateTime)
    {
        var o = ordinal;
        return Months
            .Select(m => new DateOnly(dateTime.Year, (int)m, 1))
            .Select(d => o.ToDate(d))
            .ToArray();
    }
}