namespace XTI_Core.Tests;

internal sealed class TruncatedTextTest
{
    [Test]
    public void ShouldTruncateText_WhenGreaterThanMaxLength()
    {
        var truncatedText = new TruncatedText("1234", 3);
        Assert.That(truncatedText.Value, Is.EqualTo("123"), "Should truncate text");
    }

    [Test]
    public void ShouldNotTruncateText_WhenLessThanMaxLength()
    {
        var truncatedText = new TruncatedText("1234", 5);
        Assert.That(truncatedText.Value, Is.EqualTo("1234"), "Should not truncate text");
    }

    [Test]
    public void ShouldNotTruncateText_WhenEqualToMaxLength()
    {
        var truncatedText = new TruncatedText("1234", 4);
        Assert.That(truncatedText.Value, Is.EqualTo("1234"), "Should not truncate text");
    }
}
