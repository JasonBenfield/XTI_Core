using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTI_Schedule;

namespace XTI_Core.Tests
{
    public sealed class TimeRangeTest
    {
        [Test]
        public void ShouldDeserializeTimeRange()
        {
            var timeRange = new TimeRange
            (
                new Time(10, 0, 0),
                TimeSpan.FromHours(2)
            );
            var serialized = timeRange.Serialize();
            var deserialized = TimeRange.Deserialize(serialized);
            Assert.That(deserialized.Start, Is.EqualTo(timeRange.Start));
            Assert.That(deserialized.Duration, Is.EqualTo(timeRange.Duration));
        }

        [Test]
        public void ShouldGetStartTime()
        {
            var timeRange = new TimeRange
            (
                new Time(10, 0, 0),
                TimeSpan.FromHours(2)
            );
            var startTime = timeRange.StartTime(new DateTimeOffset(new DateTime(2021, 10, 1, 13, 0, 0)));
            Assert.That(startTime, Is.EqualTo(new DateTimeOffset(new DateTime(2021, 10, 1, 10, 0, 0))));
        }

        [Test]
        public void ShouldGetEndTime()
        {
            var timeRange = new TimeRange
            (
                new Time(10, 0, 0),
                TimeSpan.FromHours(2)
            );
            var endTime = timeRange.EndTime(new DateTimeOffset(new DateTime(2021, 10, 1, 13, 0, 0)));
            Assert.That(endTime, Is.EqualTo(new DateTimeOffset(new DateTime(2021, 10, 1, 12, 0, 0))));
        }

        [Test]
        public void ShouldReturnTrue_WhenValueIsWithinTimeRange()
        {
            var timeRange = new TimeRange
            (
                new Time(10, 0, 0),
                TimeSpan.FromHours(2)
            );
            var isInTimeRange = timeRange.IsInTimeRange(new DateTimeOffset(new DateTime(2021, 10, 1, 11, 0, 0)));
            Assert.That(isInTimeRange, Is.True);
        }
    }
}
