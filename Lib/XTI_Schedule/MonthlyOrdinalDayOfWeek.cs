namespace XTI_Schedule;

public struct MonthlyOrdinalDayOfWeek
{
    public MonthlyOrdinalDayOfWeek(OrdinalWeek week, DayOfWeek day)
    {
        Week = week;
        DayOfWeek = day;
    }

    public OrdinalWeek Week { get; }
    public DayOfWeek DayOfWeek { get; }

    public DateTime ToDate(DateTime dateTime)
    {
        if (Week == OrdinalWeek.None)
        {
            throw new ArgumentException("Week cannot be None");
        }
        if (DayOfWeek == 0)
        {
            throw new ArgumentException("Day of Week cannot be None");
        }
        DateTime date;
        if (Week == OrdinalWeek.Last)
        {
            var endOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1)
                .AddMonths(1)
                .AddDays(-1);
            var current = endOfMonth;
            while (current.DayOfWeek != DayOfWeek)
            {
                current = current.AddDays(-1);
            }
            date = current;
        }
        else
        {
            var firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var current = firstOfMonth;
            while (current.DayOfWeek != DayOfWeek)
            {
                current = current.AddDays(1);
            }
            var weeks = (int)Week - 1;
            date = current.AddDays(7 * weeks);
        }
        return date;
    }
}