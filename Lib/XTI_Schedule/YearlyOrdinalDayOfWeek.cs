namespace XTI_Schedule;

public struct YearlyOrdinalDayOfWeek
{
    private readonly MonthlyOrdinalDayOfWeek ordinal;

    public YearlyOrdinalDayOfWeek(OrdinalWeek week, DayOfWeek day, params Months[] months)
    {
        ordinal = new MonthlyOrdinalDayOfWeek(week, day);
        Months = months ?? new Months[] { };
    }

    public OrdinalWeek Week { get => ordinal.Week; }
    public DayOfWeek DayOfWeek { get => ordinal.DayOfWeek; }
    public Months[] Months { get; }

    public DateTime[] ToDates(DateTime dateTime)
    {
        var o = ordinal;
        return Months
            .Select(m => new DateTime(dateTime.Year, (int)m, 1))
            .Select(d => o.ToDate(d))
            .ToArray();
    }
}