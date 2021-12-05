namespace XTI_Schedule;

public sealed class YearlyOrdinalDayOfWeekOptions
{
    private Months[] months = new Months[0];

    public OrdinalWeek Week { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public Months[] Months
    {
        get => months;
        set => months = value ?? new Months[0];
    }

    public YearlyOrdinalDayOfWeek ToOrdinalDayOfWeek() =>
        new YearlyOrdinalDayOfWeek(Week, DayOfWeek, Months);
}