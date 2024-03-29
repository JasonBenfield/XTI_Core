﻿using XTI_Core;

namespace XTI_Schedule;

public sealed class WeeklySchedule : IDaySchedule
{
    public static WeeklySchedule EveryDay()
        => new WeeklySchedule
        (
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday
        );

    public static WeeklySchedule Weekdays()
        => new WeeklySchedule
        (
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday
        );

    public static WeeklySchedule Weekend()
        => new WeeklySchedule
        (
            DayOfWeek.Sunday,
            DayOfWeek.Saturday
        );

    public WeeklySchedule(params DayOfWeek[] daysOfWeek)
    {
        this.Days = daysOfWeek;
    }

    internal DayOfWeek[] Days { get; }

    public bool IsInRange(DateOnly value) =>
        Days.Any(d => value.DayOfWeek == d);

    public DateOnly[] AllowedDates(DateRange range)
        => range.Dates()
            .Where(d => IsInRange(d))
            .ToArray();
}