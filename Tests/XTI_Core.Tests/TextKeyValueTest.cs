using NUnit.Framework;
using System.Text.Json;

namespace XTI_Core.Tests;

public sealed class TextKeyValueTest
{
    [Test]
    public void ShouldDisplayCapitalizedWord()
    {
        var key = new TestKey("one");
        Assert.That(key.DisplayText, Is.EqualTo("One"), "Should capitalize one word key");
    }

    [Test]
    public void ShouldDisplayAsMultipleCapitalizedWords()
    {
        var key = new TestKey("one_two");
        Assert.That(key.DisplayText, Is.EqualTo("One Two"), "Should capitalize two word key");
    }

    [Test]
    [
        TestCase("one", "One"),
        TestCase("one_two", "One_Two"),
        TestCase("one two", "One_Two"),
        TestCase("OneTwo", "One_Two"),
        TestCase("oneTwo", "One_Two")
    ]
    public void ShouldBeEqual(string keyValue, string otherValue)
    {
        var key = new TestKey(keyValue);
        var otherKey = new TestKey(otherValue);
        Assert.That(key, Is.EqualTo(otherKey));
    }

    [Test]
    [
        TestCase("one", "One"),
        TestCase("OneTwo", "one_two"),
        TestCase("OneTwo", "one two"),
        TestCase("OneTwo", "OneTwo"),
        TestCase("OneTwo", "oneTwo")
    ]
    public void ShouldBeEqualToString(string keyValue, string otherValue)
    {
        var key = new TestKey(keyValue);
        Assert.That(key.Equals(otherValue), Is.True);
    }

    [Test]
    [
        TestCase("one", "Two"),
        TestCase("one_two", "One_Three")
    ]
    public void ShouldNotBeEqual(string keyValue, string otherValue)
    {
        var key = new TestKey(keyValue);
        var otherKey = new TestKey(otherValue);
        Assert.That(key, Is.Not.EqualTo(otherKey));
    }

    [Test]
    [
        TestCase("one"),
        TestCase("one_two"),
        TestCase("Three Four")
    ]
    public void ShouldDeserializeTextKeyValue(string keyValue)
    {
        var key = new TestKey(keyValue);
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new TextValueJsonConverter<TestKey>());
        var serialized = JsonSerializer.Serialize(key, jsonOptions);
        Console.WriteLine(serialized);
        var deserialized = JsonSerializer.Deserialize<TestKey>(serialized);
        Assert.That(deserialized, Is.EqualTo(key));
    }

    [Test]
    [
        TestCase("one", "One"),
        TestCase("one_two", "One Two"),
        TestCase("three four", "Three Four")
    ]
    public void ShouldDeserializeTextValue(string keyValue, string displayText)
    {
        var key = new TestValue(keyValue, displayText);
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new TextValueJsonConverter<TestValue>());
        var serialized = JsonSerializer.Serialize(key, jsonOptions);
        Console.WriteLine(serialized);
        var deserialized = JsonSerializer.Deserialize<TestValue>(serialized);
        Assert.That(deserialized, Is.EqualTo(key));
    }

    [Test]
    [
        TestCase("one", "One"),
        TestCase("one_two", "One Two"),
        TestCase("three four", "Three Four")
    ]
    public void ShouldConvertTextValueFromString(string keyValue, string displayText)
    {
        var key = new TestValue(keyValue, displayText);
        var converted = new TextValueTypeConverter<TestValue>().ConvertFromString(keyValue);
        Assert.That(converted, Is.EqualTo(key));
    }

    public sealed class TestKey : TextKeyValue, IEquatable<TestKey>
    {
        public TestKey(string value) : base(value)
        {
        }

        public bool Equals(TestKey? other) => _Equals(other);
    }

    public sealed class TestValue : TextValue, IEquatable<TestValue>
    {
        public TestValue(string value, string displayText) : base(value, displayText)
        {
        }

        public bool Equals(TestValue? other) => _Equals(other);
    }
}