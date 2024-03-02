namespace XTI_Core.Tests;

internal sealed class DateOnlyTest
{
    [Test]
    public void ShouldDeserializeDateOnly()
    {
        var dateOnly = new DateOnly(2023, 9, 23);
        var serialized = XtiSerializer.Serialize(dateOnly);
        Console.WriteLine(serialized);
        var deserialized = XtiSerializer.Deserialize<DateOnly>(serialized);
        Assert.That(deserialized, Is.EqualTo(dateOnly), "Should deserialize date");
    }

    [Test]
    public void ShouldDeserializeISOString()
    {
        var deserialized = XtiSerializer.Deserialize<DateOnly>("\"2023-11-22\"");
        Assert.That
        (
            deserialized, 
            Is.EqualTo(new DateOnly(2023, 11, 22)), 
            "Should deserialize ISO string"
        );
    }
}
