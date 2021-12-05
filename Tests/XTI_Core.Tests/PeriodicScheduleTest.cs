using NUnit.Framework;
using XTI_Schedule;

namespace XTI_Core.Tests;

sealed class PeriodicScheduleTest
{
    [Test]
    public void ShouldBeInRange_WhenDateIsEqualToStartTime()
    {
        var schedule = Schedule
            .Every(3).Days().Starting(new DateTime(2021, 10, 1))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 1, 19, 35, 0)),
            Is.True,
            "Should be in range when date is equal to the start date"
        );
    }

    [Test]
    public void ShouldBeInRange_WhenDateIsOneDayIntervalPastTheStartDate()
    {
        var schedule = Schedule
            .Every(3).Days().Starting(new DateTime(2021, 10, 1))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 4, 19, 35, 0)),
            Is.True,
            "Should be in range when date is one day interval past the start date"
        );
    }

    [Test]
    public void ShouldBeInRange_WhenDateIsTwoDayIntervalsPastTheStartDate()
    {
        var schedule = Schedule
            .Every(3).Days().Starting(new DateTime(2021, 10, 1))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 7, 19, 35, 0)),
            Is.True,
            "Should be in range when date is two day intervals past the start date"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsBeforeTheStartDate()
    {
        var schedule = Schedule
            .Every(3).Days().Starting(new DateTime(2021, 10, 1))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTimeOffset(new DateTime(2021, 9, 28, 19, 35, 0))),
            Is.False,
            "Should not be in range when date is before the start date"
        );
    }

    [Test]
    public void ShouldBeInRange_WhenDateIsOneWeekIntervalPastTheStartDate()
    {
        var schedule = Schedule
            .Every(2).Weeks().Starting(new DateTime(2021, 10, 1))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 15, 19, 35, 0)),
            Is.True,
            "Should be in range when date is one week interval past the start date"
        );
    }

    [Test]
    public void ShouldBeInRange_WhenDateIsTwoWeekIntervalsPastTheStartDate()
    {
        var schedule = Schedule
            .Every(2).Weeks().Starting(new DateTime(2021, 10, 1))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2021, 10, 29, 19, 35, 0)),
            Is.True,
            "Should be in range when date is two week intervals past the start date"
        );
    }

    [Test]
    public void ShouldBeInRange_WhenDateIsOneMonthIntervalPastTheStartDate()
    {
        var schedule = Schedule
            .Every(3).Months().Starting(new DateTime(2021, 10, 5))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2022, 1, 5, 19, 35, 0)),
            Is.True,
            "Should be in range when date is one month interval past the start date"
        );
    }

    [Test]
    public void ShouldBeInRange_WhenDateIsTwoMonthIntervalsPastTheStartDate()
    {
        var schedule = Schedule
            .Every(3).Months().Starting(new DateTime(2021, 10, 5))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2022, 4, 5, 19, 35, 0)),
            Is.True,
            "Should be in range when date is two month intervals past the start date"
        );
    }

    [Test]
    public void ShouldNotBeInRange_WhenDateIsNotAllowed()
    {
        var schedule = Schedule
            .Every(3).Months().Starting(new DateTime(2021, 10, 5))
            .At(TimeRange.From(19, 35).ForOneHour());
        Assert.That
        (
            schedule.IsInSchedule(new DateTime(2022, 4, 4, 19, 35, 0)),
            Is.False,
            "Should not be in range when date is not allowed"
        );
    }

    [Test]
    public void ShouldGetAllowedDates()
    {
        var schedule = Schedule
            .Every(3).Months().Starting(new DateTime(2021, 10, 5))
            .At(TimeRange.From(19, 35).ForOneHour());
        var allowedDateTimeRanges = schedule.DateTimeRanges
        (
            DateRange.Between
            (
                new DateTime(2021, 10, 1),
                new DateTime(2022, 4, 6)
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
                            new DateTime(2021, 10, 5, 19, 35, 0),
                            new DateTime(2021, 10, 5, 20, 35, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2022, 1, 5, 19, 35, 0),
                            new DateTime(2022, 1, 5, 20, 35, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2022, 4, 5, 19, 35, 0),
                            new DateTime(2022, 4, 5, 20, 35,  0)
                        )
                }
            )
        );
    }
}