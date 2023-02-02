using NUnit.Framework;

namespace XTI_Core.Tests;

public sealed class TimeRangeTest
{
    [Test]
    public void ShouldDeserializeTimeRange()
    {
        var timeRange = new TimeRange
        (
            new TimeOnly(10, 0, 0),
            TimeSpan.FromHours(2)
        );
        var serialized = XtiSerializer.Serialize(timeRange);
        Console.WriteLine($"Serialized Time Range: {serialized}");
        var deserialized = XtiSerializer.Deserialize<TimeRange>(serialized);
        Assert.That(deserialized.Start, Is.EqualTo(timeRange.Start));
        Assert.That(deserialized.Duration, Is.EqualTo(timeRange.Duration));
    }

    [Test]
    public void ShouldGetStartTime()
    {
        var timeRange = new TimeRange
        (
            new TimeOnly(10, 0, 0),
            TimeSpan.FromHours(2)
        );
        var startTime = timeRange.StartTime(new DateOnly(2021, 10, 1));
        Assert.That(startTime, Is.EqualTo(new DateTimeOffset(new DateTime(2021, 10, 1, 10, 0, 0))));
    }

    [Test]
    public void ShouldGetEndTime()
    {
        var timeRange = new TimeRange
        (
            new TimeOnly(10, 0, 0),
            TimeSpan.FromHours(2)
        );
        var endTime = timeRange.EndTime(new DateOnly(2021, 10, 1));
        Assert.That(endTime, Is.EqualTo(new DateTimeOffset(new DateTime(2021, 10, 1, 12, 0, 0))));
    }

    [Test]
    public void ShouldReturnTrue_WhenValueIsWithinTimeRange()
    {
        var timeRange = new TimeRange
        (
            new TimeOnly(10, 0, 0),
            TimeSpan.FromHours(2)
        );
        var isInTimeRange = timeRange.IsInTimeRange(new DateTimeOffset(new DateTime(2021, 10, 1, 11, 0, 0)));
        Assert.That(isInTimeRange, Is.True);
    }
}