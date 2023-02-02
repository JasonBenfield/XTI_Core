using XTI_Core;

namespace XTI_Schedule;

public sealed class ScheduleBuilder
{
    public NextScheduleBuilder On(params DayOfWeek[] daysOfWeek) => new NextScheduleBuilder(new WeeklySchedule(daysOfWeek));

    public NextScheduleBuilder EveryDay() => new NextScheduleBuilder(WeeklySchedule.EveryDay());

    public NextScheduleBuilder Weekdays() => new NextScheduleBuilder(WeeklySchedule.Weekdays());

    public NextScheduleBuilder Weekends() => new NextScheduleBuilder(WeeklySchedule.Weekend());

    public NextScheduleBuilder On(params MonthDay[] days) => new NextScheduleBuilder(new MonthlySchedule(days));

    public NextScheduleBuilder On(params YearDay[] days) => new NextScheduleBuilder(new YearlySchedule(days));

    public OrdinalScheduleBuilder First(DayOfWeek day) => new OrdinalScheduleBuilder(new NextScheduleBuilder(), OrdinalWeek.First, day);

    public OrdinalScheduleBuilder Second(DayOfWeek day) => new OrdinalScheduleBuilder(new NextScheduleBuilder(), OrdinalWeek.Second, day);

    public OrdinalScheduleBuilder Third(DayOfWeek day) => new OrdinalScheduleBuilder(new NextScheduleBuilder(), OrdinalWeek.Third, day);

    public OrdinalScheduleBuilder Fourth(DayOfWeek day) => new OrdinalScheduleBuilder(new NextScheduleBuilder(), OrdinalWeek.Fourth, day);

    public OrdinalScheduleBuilder Last(DayOfWeek day) => new OrdinalScheduleBuilder(new NextScheduleBuilder(), OrdinalWeek.Last, day);

    public NextPeriodicScheduleBuilder EveryWeek() => new PeriodicScheduleBuilder(new NextScheduleBuilder(), 1).Weeks();

    public NextPeriodicScheduleBuilder EveryMonth() => new PeriodicScheduleBuilder(new NextScheduleBuilder(), 1).Months();

    public NextPeriodicScheduleBuilder EveryYear() => new PeriodicScheduleBuilder(new NextScheduleBuilder(), 1).Years();

    public PeriodicScheduleBuilder Every(int frequency) => new PeriodicScheduleBuilder(new NextScheduleBuilder(), frequency);

}

public sealed class NextScheduleBuilder
{
    private readonly List<IDaySchedule> daySchedules = new();

    public NextScheduleBuilder(params IDaySchedule[] daySchedules)
    {
        this.daySchedules.AddRange(daySchedules ?? new IDaySchedule[0]);
    }

    public NextScheduleBuilder AndOn(params DayOfWeek[] daysOfWeek)
        => Add(new WeeklySchedule(daysOfWeek));

    public NextScheduleBuilder AndEveryDay() => Add(WeeklySchedule.EveryDay());

    public NextScheduleBuilder AndWeekdays() => Add(WeeklySchedule.Weekdays());

    public NextScheduleBuilder AndWeekends() => Add(WeeklySchedule.Weekend());

    public NextScheduleBuilder AndOn(params MonthDay[] days) => Add(new MonthlySchedule(days));

    public OrdinalScheduleBuilder AndTheFirst(DayOfWeek day) => new OrdinalScheduleBuilder(this, OrdinalWeek.First, day);

    public OrdinalScheduleBuilder AndTheSecond(DayOfWeek day) => new OrdinalScheduleBuilder(this, OrdinalWeek.Second, day);

    public OrdinalScheduleBuilder AndTheThird(DayOfWeek day) => new OrdinalScheduleBuilder(this, OrdinalWeek.Third, day);

    public OrdinalScheduleBuilder AndTheFourth(DayOfWeek day) => new OrdinalScheduleBuilder(this, OrdinalWeek.Fourth, day);

    public OrdinalScheduleBuilder AndTheLast(DayOfWeek day) => new OrdinalScheduleBuilder(this, OrdinalWeek.Last, day);

    public NextPeriodicScheduleBuilder AndEveryWeek() => new PeriodicScheduleBuilder(this, 1).Weeks();

    public NextPeriodicScheduleBuilder AndEveryMonth() => new PeriodicScheduleBuilder(this, 1).Months();

    public NextPeriodicScheduleBuilder AndEveryYear() => new PeriodicScheduleBuilder(this, 1).Years();

    public PeriodicScheduleBuilder AndEvery(int frequency) => new PeriodicScheduleBuilder(this, frequency);

    internal NextScheduleBuilder Add(IDaySchedule daySchedule)
    {
        daySchedules.Add(daySchedule);
        return this;
    }

    public Schedule At(params TimeRange[] timeRanges) => 
        new Schedule(daySchedules.ToArray(), timeRanges);
}