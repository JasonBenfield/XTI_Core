using NUnit.Framework;
using XTI_Schedule;

namespace XTI_Core.Tests;

sealed class MonthlyOrdinalScheduleTest
{
    [Test]
    public void ShouldBeInRangeOnFirstWednesday_WhenFirstWednesdayIsAllowed()
    {
        var schedule = Schedule
            .First(DayOfWeek.Wednesday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 6, 21, 15, 0)),
            Is.True,
            "Should be in range on first Wednesday when first Wednesday is allowed"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 1, 21, 15, 0)),
            Is.True,
            "Should be in range on first Wednesday when first Wednesday is allowed"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsNotOnAllowedFirstDayOfWeek()
    {
        var schedule = Schedule
            .First(DayOfWeek.Wednesday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 5, 21, 15, 0)),
            Is.False,
            "Should not be in range when date is not on allowed first day of week"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTimeOffset(new DateTime(2021, 9, 2, 21, 15, 0))),
            Is.False,
            "Should not be in range when date is not on allowed first day of week"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsOnAllowedFirstDayOfWeekButTimeIsNotInTimeRange()
    {
        var schedule = Schedule
            .First(DayOfWeek.Wednesday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 6, 22, 05, 0)),
            Is.False,
            "Should not be in range when date is on allowed first day of week but time is not within range"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 1, 20, 15, 0)),
            Is.False,
            "Should not be in range when date is on allowed first day of week but time is not within range"
        );
    }

    [Test]
    public void ShouldBeInRangeOnSecondThursday_WhenSecondThursdayIsAllowed()
    {
        var schedule = Schedule
            .Second(DayOfWeek.Thursday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 14, 21, 15, 0)),
            Is.True,
            "Should be in range on second Thursday when second Thursday is allowed"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 9, 21, 15, 0)),
            Is.True,
            "Should be in range on second Thursday when second Thursday is allowed"
        );
    }

    [Test]
    public void ShouldBeInRangeOnThirdTuesday_WhenThirdTuesdayIsAllowed()
    {
        var schedule = Schedule
            .Third(DayOfWeek.Tuesday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 19, 21, 15, 0)),
            Is.True,
            "Should be in range on third Tuesday when third Tuesday is allowed"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 21, 21, 15, 0)),
            Is.True,
            "Should be in range on third Tuesday when third Tuesday is allowed"
        );
    }

    [Test]
    public void ShouldBeInRangeOnFourthMonday_WhenFourthMondayIsAllowed()
    {
        var schedule = Schedule
            .Fourth(DayOfWeek.Monday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 25, 21, 15, 0)),
            Is.True,
            "Should be in range on fourth Monday when fourth Monday is allowed"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 27, 21, 15, 0)),
            Is.True,
            "Should be in range on fourth Monday when fourth Monday is allowed"
        );
    }

    [Test]
    public void ShouldBeInRangeOnLastSaturday_WhenLastSaturdayIsAllowed()
    {
        var schedule = Schedule
            .Last(DayOfWeek.Saturday).OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 30, 21, 15, 0)),
            Is.True,
            "Should be in range on last Saturday when last Saturday is allowed"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 25, 21, 15, 0)),
            Is.True,
            "Should be in range on last Saturday when last Saturday is allowed"
        );
    }

    [Test]
    public void ShouldGetAllowedDateTimeRanges()
    {
        var schedule = Schedule
            .First(DayOfWeek.Monday)
            .OfEveryMonth()
            .AndTheThird(DayOfWeek.Wednesday)
            .OfEveryMonth()
            .AndTheLast(DayOfWeek.Saturday)
            .OfEveryMonth()
            .At(TimeRange.From(21, 0).ForOneHour());
        var ranges = schedule.DateTimeRanges
        (
            DateRange.Between
            (
                new DateTime(2021, 9, 24),
                new DateTime(2021, 10, 21)
            )
        );
        Assert.That
        (
            ranges,
            Is.EqualTo
            (
                new[]
                {
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 9, 25, 21, 0, 0),
                            new DateTime(2021, 9, 25, 22, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 4, 21, 0, 0),
                            new DateTime(2021, 10, 4, 22, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 20, 21, 0, 0),
                            new DateTime(2021, 10, 20, 22, 0, 0)
                        )
                }
            ),
            "Should get allowed date time ranges"
        );
    }
}