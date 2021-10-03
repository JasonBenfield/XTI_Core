using NUnit.Framework;
using System;

namespace XTI_Core.Tests
{
    public sealed class DateRangeTest
    {
        [Test]
        public void ShouldBeInRange_WhenValueIsBetweenStartAndEnd()
        {
            var value = DateTimeOffset.Now;
            var start = value.AddDays(-1);
            var end = value.AddDays(1);
            var dateRange = DateRange.Between(start, end);
            Assert.That(dateRange.IsInRange(start), Is.True);
            Assert.That(dateRange.IsInRange(start.AddDays(-1)), Is.False);
            Assert.That(dateRange.IsInRange(value), Is.True);
            Assert.That(dateRange.IsInRange(end), Is.True);
            Assert.That(dateRange.IsInRange(end.AddDays(1)), Is.False);
        }

        [Test]
        public void ShouldBeInRange_WhenValueIsOnOrAfterTheStartDate()
        {
            var value = DateTimeOffset.Now;
            var start = value.AddDays(-1);
            var dateRange = DateRange.OnOrAfter(start);
            Assert.That(dateRange.IsInRange(start), Is.True);
            Assert.That(dateRange.IsInRange(value), Is.True);
            Assert.That(dateRange.IsInRange(start.AddDays(-1)), Is.False);
        }

        [Test]
        public void ShouldBeInRange_WhenValueIsOnOrBeforeTheEndTime()
        {
            var value = DateTimeOffset.Now;
            var end = value.AddDays(1);
            var dateRange = DateRange.OnOrBefore(end);
            Assert.That(dateRange.IsInRange(end), Is.True);
            Assert.That(dateRange.IsInRange(value), Is.True);
            Assert.That(dateRange.IsInRange(end.AddDays(1)), Is.False);
        }

        [Test]
        public void ShouldBeInRange_WhenValueIsOnTheGivenTime()
        {
            var value = DateTimeOffset.Now;
            var exact = value;
            var dateRange = DateRange.On(exact);
            Assert.That(dateRange.IsInRange(exact), Is.True);
            Assert.That(dateRange.IsInRange(value), Is.True);
            Assert.That(dateRange.IsInRange(exact.Date.AddDays(-1)), Is.False);
            Assert.That(dateRange.IsInRange(exact.Date.AddDays(1)), Is.False);
            Assert.That(dateRange.IsInRange(exact.Date.AddDays(1).AddMilliseconds(-1)), Is.True);
        }

        [Test]
        public void ShouldGetDatesInRange()
        {
            var dateRange = DateRange.Between(new DateTimeOffset(new DateTime(2021, 9, 29)), new DateTimeOffset(new DateTime(2021, 10, 2)));
            var dates = dateRange.Dates();
            Assert.That
            (
                dates,
                Is.EqualTo
                (
                    new[]
                    {
                        new DateTime(2021, 9, 29),
                        new DateTime(2021, 9, 30),
                        new DateTime(2021, 10, 1),
                        new DateTime(2021, 10, 2)
                    }
                )
            );
        }
    }
}
