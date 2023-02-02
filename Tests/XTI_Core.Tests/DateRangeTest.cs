using NUnit.Framework;

namespace XTI_Core.Tests;

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
    public void ShouldBeInRange_WhenValueIsFromStartForDuration()
    {
        var value = DateTimeOffset.Now;
        var start = value.AddDays(-1);
        var end = value.AddDays(1);
        var dateRange = DateRange.From(start).For(2).Days();
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
        var dateRange = DateRange.Between(new DateOnly(2021, 9, 29), new DateOnly(2021, 10, 2));
        var dates = dateRange.Dates();
        Assert.That
        (
            dates,
            Is.EqualTo
            (
                new[]
                {
                    new DateOnly(2021, 9, 29),
                    new DateOnly(2021, 9, 30),
                    new DateOnly(2021, 10, 1),
                    new DateOnly(2021, 10, 2)
                }
            )
        );
    }

    [Test]
    public void ShouldDeserializeDateRange()
    {
        var start = new DateOnly(2021, 9, 29);
        var end = new DateOnly(2021, 10, 2);
        var dateRange = DateRange.Between(start, end);
        var serialized = XtiSerializer.Serialize(dateRange);
        Console.WriteLine($"Serialized Date Range: {serialized}");
        var deserialized = XtiSerializer.Deserialize(serialized, DateRange.All);
        Assert.That(deserialized.Start, Is.EqualTo(start));
        Assert.That(deserialized.End, Is.EqualTo(end));
    }

}