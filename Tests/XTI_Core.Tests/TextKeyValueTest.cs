using NUnit.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTI_Core.Tests;

public sealed class TextKeyValueTest
{
    [Test]
    public void ShouldParseCapitalizedWord()
    {
        var key = TestKey.Parse("one");
        Assert.That(key.DisplayText, Is.EqualTo("One"), "Should capitalize one word key");
    }

    [Test]
    public void ShouldParseAsMultipleCapitalizedWords()
    {
        var key = TestKey.Parse("one_two");
        Assert.That(key.DisplayText, Is.EqualTo("One Two"), "Should capitalize two word key");
    }

    [Test]
    public void ShouldNotChangeWordBeginningWithTwoOrMoreCapitalLettersFollowedByLowerCase()
    {
        var key = new TestKey("ONe Two");
        Assert.That(key.DisplayText, Is.EqualTo("ONe Two"), "Should not change word beginning with 2 or more capital letters followed by lower case");
    }

    [Test]
    public void ShouldSplitWordByCamelCase()
    {
        var key = new TestKey("OneTwo");
        Assert.That(key.DisplayText, Is.EqualTo("One Two"), "Should split camel cased words");
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

    [Test]
    public void ShouldOverrideWithJsonConverterAttribute()
    {
        var key = new AnotherTestValue(23);
        var serialized = XtiSerializer.Serialize(key);
        var deserialized = XtiSerializer.Deserialize(serialized, ()=>new AnotherTestValue(0));
        Assert.That(deserialized, Is.EqualTo(key));
    }

    public sealed class TestKey : TextKeyValue, IEquatable<TestKey>
    {
        public static TestKey Parse(string value) => new TestKey(value, "");

        public TestKey(string value) : base(value)
        {
        }

        private TestKey(string value, string displayText) : base(value, displayText)
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

    public sealed class AnotherTestValueJsonConverter : JsonConverter<AnotherTestValue>
    {
        public override AnotherTestValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            AnotherTestValue? result = null;
            if (reader.TokenType == JsonTokenType.String)
            {
                result = AnotherTestValue.Parse(reader.GetString() ?? "");
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, AnotherTestValue value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }

    [JsonConverter(typeof(AnotherTestValueJsonConverter))]
    public sealed class AnotherTestValue : TextValue, IEquatable<AnotherTestValue>
    {
        public static AnotherTestValue Parse(string text) => new AnotherTestValue(int.Parse(text.Replace("Value", "")));

        public AnotherTestValue(int value) : base($"Value{value}")
        {
        }

        public bool Equals(AnotherTestValue? other) => _Equals(other);
    }
}