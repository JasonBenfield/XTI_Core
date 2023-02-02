using NUnit.Framework;
using XTI_Schedule;

namespace XTI_Core.Tests;

sealed class WeeklyScheduleTest
{
    [Test]
    public void ShouldBeInRange_WhenDateIsOnAllowedDayAndTimeIsWithinTheTimeRange()
    {
        var schedule = Schedule
            .On(DayOfWeek.Monday)
            .At(TimeRange.From(10, 13).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 4, 10, 45, 0)),
            Is.True,
            "Should be in range when date is on an allowed day and time is within the range"
        );
    }

    [Test]
    public void ShouldBeInRangeWithMultipleTimeRanges()
    {
        var schedule = Schedule
            .On(DayOfWeek.Monday)
            .At(TimeRange.From(10, 13).ForOneHour(), TimeRange.From(14, 13).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 4, 10, 45, 0)),
            Is.True,
            "Should be in range with multiple time ranges"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 4, 14, 45, 0)),
            Is.True,
            "Should be in range with multiple time ranges"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsNotOnAllowedDayButTimeIsWithinTheTimeRange()
    {
        var schedule = Schedule
            .On(DayOfWeek.Monday)
            .At(TimeRange.From(10, 13).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 5, 10, 45, 0)),
            Is.False,
            "Should not be in range when date is not on an allowed day but time is within the range"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsOnAllowedDayButTimeIsNotWithinTheTimeRange()
    {
        var schedule = Schedule
            .On(DayOfWeek.Monday)
            .At(TimeRange.From(10, 13).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 4, 11, 15, 0)),
            Is.False,
            "Should not be in range when date is on an allowed day but time is not within the range"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsNotOnAllowedDayAndTimeIsNotWithinTheTimeRange()
    {
        var schedule = Schedule
            .On(DayOfWeek.Monday)
            .At(TimeRange.From(10, 13).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 5, 11, 15, 0)),
            Is.False,
            "Should not be in range when date is not on an allowed day and time is not within the range"
        );
    }

    [Test]
    public void ShouldGetAllowedDateTimeRanges()
    {
        var schedule = Schedule
            .On(DayOfWeek.Monday, DayOfWeek.Friday)
            .At(TimeRange.From(10, 13).ForOneHour());
        var allowedTimes = schedule.DateTimeRanges
        (
            DateRange.Between
            (
                new DateTime(2021, 10, 1),
                new DateTime(2021, 10, 11)
            )
        );
        Assert.That
        (
            allowedTimes,
            Is.EqualTo
            (
                new[]
                {
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 1, 10, 13, 0),
                            new DateTime(2021, 10, 1, 11, 13, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 4, 10, 13, 0),
                            new DateTime(2021, 10, 4, 11, 13, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 8, 10, 13, 0),
                            new DateTime(2021, 10, 8, 11, 13, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 11, 10, 13, 0),
                            new DateTime(2021, 10, 11, 11, 13, 0)
                        )
                }
            )
        );
    }
}