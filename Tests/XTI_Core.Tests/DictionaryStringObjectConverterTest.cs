using XTI_Core.Extensions;

namespace XTI_Core.Tests;

internal sealed class DictionaryStringObjectConverterTest
{
    [Test]
    public void ShouldDeserializeDictionaryStringObject()
    {
        var dict = new Dictionary<string, object>
        {
            { "1", 1 },
            { "2", "test" },
            { "3", true },
            { "4", 2.34 },
            { "5", new [] { "Item1", "Item2" } },
            { 
                "6",
                new Dictionary<string, object>
                {
                    { "Key1", 11 },
                    { "Key2", true },
                    { "Key3", 11.22 },
                    { "Key4", "Value4" }
                }
            }
        };
        var serialized = XtiSerializer.Serialize(dict);
        var deserialized = XtiSerializer.Deserialize<Dictionary<string, object>>(serialized);
        Assert.That(deserialized["1"], Is.TypeOf<decimal>());
        Assert.That(deserialized["1"], Is.EqualTo(1M));
        Assert.That(deserialized["2"], Is.EqualTo("test"));
        Assert.That(deserialized["3"], Is.True);
        Assert.That(deserialized["4"], Is.TypeOf<decimal>());
        Assert.That(deserialized["4"], Is.EqualTo(2.34M));
        Assert.That(deserialized["5"], Is.EqualTo(new[] { "Item1", "Item2" }));
        Assert.That
        (
            deserialized["6"], 
            Is.EqualTo
            (
                new Dictionary<string, object>
                {
                    { "Key1", 11 },
                    { "Key2", true },
                    { "Key3", 11.22 },
                    { "Key4", "Value4" }
                }
            )
        );
        deserialized.WriteToConsole();
    }

    [Test]
    public void ShouldDeserializeDictionaryStringString()
    {
        var dict = new Dictionary<string, string>
        {
            { "1", "Value1" },
            { "2", "Value2" },
            { "3", "Value3" },
            { "4", "Value4" }
        };
        var serialized = XtiSerializer.Serialize(dict);
        var deserialized = XtiSerializer.Deserialize<Dictionary<string, string>>(serialized);
        Assert.That
        (
            dict,
            Is.EqualTo
            (
                new Dictionary<string, string>
                {
                    { "1", "Value1" },
                    { "2", "Value2" },
                    { "3", "Value3" },
                    { "4", "Value4" }
                }
            )
        );
        deserialized.WriteToConsole();
    }
}