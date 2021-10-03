using NUnit.Framework;
using System;
using XTI_Schedule;

namespace XTI_Core.Tests
{
    sealed class MonthlyScheduleTest
    {
        [Test]
        public void ShouldBeInRange_WhenDateIsOnAllowedDayOfTheMonth()
        {
            var schedule = Schedule
                .On(new MonthDay(12))
                .At(TimeRange.From(22, 0).ForOneHour());
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 10, 12, 22, 30, 0)),
                Is.True,
                "Should be in range when date is on an allowed day of the month"
            );
        }

        [Test]
        public void ShouldBeInRangeOnLastDayOfTheMonth()
        {
            var schedule = Schedule
                .On(MonthDay.LastDay)
                .At(TimeRange.From(22, 0).ForOneHour());
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 9, 30, 22, 30, 0)),
                Is.True,
                "Should be in range on last day of the month when last day of the month is allowed"
            );
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 10, 31, 22, 30, 0)),
                Is.True,
                "Should be in range on last day of the month when last day of the month is allowed"
            );
        }

        [Test]
        public void ShouldNotBeInRange_WhenDateIsNotOnAllowedDayOfTheMonth()
        {
            var schedule = Schedule
                .On(new MonthDay(12))
                .At(TimeRange.From(22, 0).ForOneHour());
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 10, 13, 22, 30, 0)),
                Is.False,
                "Should not be in range when date is not on an allowed day of the month"
            );
        }

        [Test]
        public void ShouldNotBeInRange_WhenDateIsOnAllowedDayOfTheMonthButNotInAnAllowedTimeRange()
        {
            var schedule = Schedule
                .On(new MonthDay(12))
                .At(TimeRange.From(22, 0).ForOneHour());
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 10, 12, 23, 30, 0)),
                Is.False,
                "Should not be in range when date is on an allowed day of the month but not in an allowed time range"
            );
        }

        [Test]
        public void ShouldGetDateTimeRanges()
        {
            var schedule = Schedule
                .On(new MonthDay(1), new MonthDay(12), new MonthDay(15))
                .At(TimeRange.From(22, 0).ForOneHour());
            var ranges = schedule.DateTimeRanges
            (
                DateRange.Between
                (
                    new DateTime(2021, 10, 2),
                    new DateTime(2021, 10, 20)
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
                            new DateTime(2021, 10, 12, 22, 0, 0),
                            new DateTime(2021, 10, 12, 23, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 15, 22, 0, 0),
                            new DateTime(2021, 10, 15, 23, 0, 0)
                        )
                    }
                ),
                "Should be in range when date is on an allowed day of the month"
            );
        }

    }
}
