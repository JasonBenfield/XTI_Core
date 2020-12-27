using NUnit.Framework;
using System;

namespace XTI_Core.Tests
{
    public sealed class TimeRangeTest
    {
        [Test]
        public void ShouldBeInRange_WhenValueIsBetweenStartAndEnd()
        {
            var value = DateTimeOffset.Now;
            var start = value.AddHours(-1);
            var end = value.AddHours(1);
            var timeRange = TimeRange.Between(start, end);
            Assert.That(timeRange.IsInRange(start), Is.True);
            Assert.That(timeRange.IsInRange(start.AddHours(-1)), Is.False);
            Assert.That(timeRange.IsInRange(value), Is.True);
            Assert.That(timeRange.IsInRange(end), Is.True);
            Assert.That(timeRange.IsInRange(end.AddHours(1)), Is.False);
        }

        [Test]
        public void ShouldBeInRange_WhenValueIsOnOrAfterTheStartDate()
        {
            var value = DateTimeOffset.Now;
            var start = value.AddHours(-1);
            var timeRange = TimeRange.OnOrAfter(start);
            Assert.That(timeRange.IsInRange(start), Is.True);
            Assert.That(timeRange.IsInRange(value), Is.True);
            Assert.That(timeRange.IsInRange(start.AddHours(-1)), Is.False);
        }

        [Test]
        public void ShouldBeInRange_WhenValueIsOnOrBeforeTheEndTime()
        {
            var value = DateTimeOffset.Now;
            var end = value.AddHours(1);
            var timeRange = TimeRange.OnOrBefore(end);
            Assert.That(timeRange.IsInRange(end), Is.True);
            Assert.That(timeRange.IsInRange(value), Is.True);
            Assert.That(timeRange.IsInRange(end.AddHours(1)), Is.False);
        }
    }
}
