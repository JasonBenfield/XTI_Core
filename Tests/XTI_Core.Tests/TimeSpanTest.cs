namespace XTI_Core.Tests;

internal sealed class TimeSpanTest
{
    [Test]
    public void ShouldDeserializeTimeSpan()
    {
        var timeSpan = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(5));
        var serialized = XtiSerializer.Serialize(timeSpan);
        Console.WriteLine(serialized);
        var deserialized = XtiSerializer.Deserialize<TimeSpan>(serialized);
        Assert.That(deserialized, Is.EqualTo(timeSpan), "Should deserialize time span");
    }
}
