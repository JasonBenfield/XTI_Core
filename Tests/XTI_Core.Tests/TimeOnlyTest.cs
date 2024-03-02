namespace XTI_Core.Tests;

internal sealed class TimeOnlyTest
{
    [Test]
    public void ShouldDeserializeTimeOnly()
    {
        var timeOnly = new TimeOnly(13, 35);
        var serialized = XtiSerializer.Serialize(timeOnly);
        Console.WriteLine(serialized);
        var deserialized = XtiSerializer.Deserialize<TimeOnly>(serialized);
        Assert.That(deserialized, Is.EqualTo(timeOnly), "Should deserialize time span");
    }

    [Test]
    public void ShouldDeserializeISOString()
    {
        var deserialized = XtiSerializer.Deserialize<TimeOnly>("\"10:45:12.1230000\"");
        Assert.That
        (
            deserialized, 
            Is.EqualTo(new TimeOnly(10, 45, 12, 123)), 
            "Should deserialize ISO string"
        );
    }
}
