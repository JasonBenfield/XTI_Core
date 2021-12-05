using NUnit.Framework;
using XTI_Schedule;

namespace XTI_Core.Tests;

sealed class YearlyScheduleTest
{
    [Test]
    public void ShouldBeInRange_WhenDateIsOnAllowedDayAndTimeIsWithinTheTimeRange()
    {
        var schedule = Schedule
            .On(new YearDay(4, 18), new YearDay(9, 7))
            .At(TimeRange.From(10, 13).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 4, 18, 10, 45, 0)),
            Is.True,
            "Should be in range when date is on an allowed day and time is within the range"
        );
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 9, 7, 10, 45, 0)),
            Is.True,
            "Should be in range when date is on an allowed day and time is within the range"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsNotOnAllowedDayButTimeIsWithinTheTimeRange()
    {
        var schedule = Schedule
            .On(new YearDay(4, 18), new YearDay(9, 7))
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
            .On(new YearDay(4, 18), new YearDay(9, 7))
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
            .On(new YearDay(4, 18), new YearDay(9, 7))
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
            .On(new YearDay(4, 18), new YearDay(9, 7))
            .At(TimeRange.From(10, 13).ForOneHour());
        var allowedDateTimeRanges = schedule.DateTimeRanges
        (
            DateRange.Between
            (
                new DateTime(2021, 9, 1),
                new DateTime(2021, 10, 11)
            )
        );
        Assert.That
        (
            allowedDateTimeRanges,
            Is.EqualTo
            (
                new[]
                {
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 9, 7, 10, 13, 0),
                            new DateTime(2021, 9, 7, 11, 13, 0)
                        )
                }
            )
        );
    }
}