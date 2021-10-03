using NUnit.Framework;
using System;
using XTI_Schedule;

namespace XTI_Core.Tests
{
    sealed class YearlyOrdinalScheduleTest
    {
        [Test]
        public void ShouldBeInRangeOnThirdWednesday_WhenThirdWednesdayIsAllowedAndMonthIsAllowed()
        {
            var schedule = Schedule
                .Third(DayOfWeek.Wednesday)
                .Of(Months.January, Months.June)
                .At(TimeRange.From(21, 0).ForOneHour());
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 1, 20, 21, 15, 0)),
                Is.True,
                "Should be in range on third Wednesday when third Wednesday is allowed"
            );
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 6, 16, 21, 15, 0)),
                Is.True,
                "Should be in range on third Wednesday when third Wednesday is allowed"
            );
        }

        [Test]
        public void ShouldNotBeInRangeOnFirstWednesday_WhenFirstWednesdayIsAllowed()
        {
            var schedule = Schedule
                .Third(DayOfWeek.Wednesday)
                .Of(Months.January, Months.June)
                .At(TimeRange.From(21, 0).ForOneHour());
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 5, 19, 21, 15, 0)),
                Is.False,
                "Should be in range on third Wednesday when third Wednesday is allowed but month is not allowed"
            );
            Assert.That
            (
                schedule.IsInSchedule(new DateTime(2021, 10, 20, 21, 15, 0)),
                Is.False,
                "Should not be in range on third Wednesday when third Wednesday is allowed but month is not allowed"
            );
        }

        [Test]
        public void ShouldGetAllowedDateTimeRanges()
        {
            var schedule = Schedule
                .First(DayOfWeek.Monday).Of(Months.January)
                .AndTheThird(DayOfWeek.Wednesday).Of(Months.June)
                .AndTheLast(DayOfWeek.Saturday).Of(Months.October)
                .At(TimeRange.From(21, 0).ForOneHour());
            var ranges = schedule.DateTimeRanges
            (
                DateRange.Between
                (
                    new DateTime(2021, 5, 1),
                    new DateTime(2022, 2, 1)
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
                            new DateTime(2021, 6, 16, 21, 0, 0),
                            new DateTime(2021, 6, 16, 22, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 30, 21, 0, 0),
                            new DateTime(2021, 10, 30, 22, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2022, 1, 3, 21, 0, 0),
                            new DateTime(2022, 1, 3, 22, 0, 0)
                        )
                    }
                ),
                "Should get allowed date time ranges"
            );
        }
    }
}
