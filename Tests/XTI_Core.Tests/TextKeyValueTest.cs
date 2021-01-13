using NUnit.Framework;
using System;

namespace XTI_Core.Tests
{
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
            TestCase("one", "Two"),
            TestCase("one_two", "One_Three")
        ]
        public void ShouldNotBeEqual(string keyValue, string otherValue)
        {
            var key = new TestKey(keyValue);
            var otherKey = new TestKey(otherValue);
            Assert.That(key, Is.Not.EqualTo(otherKey));
        }

        public sealed class TestKey : TextKeyValue, IEquatable<TextKeyValue>
        {
            public TestKey(string value) : base(value)
            {
            }

            public bool Equals(TextKeyValue other) => _Equals(other);
        }
    }
}
