using NUnit.Framework;

namespace XTI_Core.Tests;

internal sealed class NumericValueTest
{
    [Test]
    public void ShouldGetValueByNumericValue()
    {
        var employeeType = EmployeeType.Values.Value(10);
        Assert.That(employeeType, Is.EqualTo(EmployeeType.Values.Temp), "Should get value by numeric value");
    }

    [Test]
    public void ShouldGetValueByDisplayText()
    {
        var employeeType = EmployeeType.Values.Value("permanent");
        Assert.That(employeeType, Is.EqualTo(EmployeeType.Values.Permanent), "Should get value by display text");
    }

    [Test]
    public void ShouldFormatCamelCasedWords()
    {
        Assert.That
        (
            EmployeeType.Values.FullTime.DisplayText,
            Is.EqualTo("Full Time"),
            "Should format camel cased words"
        );
    }

    [Test]
    public void ShouldGetValueByDisplayTextWithSpaces()
    {
        Assert.That
        (
            EmployeeType.Values.Value("full time"),
            Is.EqualTo(EmployeeType.Values.FullTime),
            "Should get value by display text with spaces"
        );
    }

    [Test]
    public void ShouldGetValueByDisplayTextWithoutSpaces()
    {
        Assert.That
        (
            EmployeeType.Values.Value("fullTime"),
            Is.EqualTo(EmployeeType.Values.FullTime),
            "Should get value by display text with spaces"
        );
    }

    [Test]
    public void ShouldGetAllValues()
    {
        Assert.That
        (
            EmployeeType.Values.GetAll(),
            Is.EquivalentTo
            (
                new[]
                {
                    EmployeeType.Values.None,
                    EmployeeType.Values.Temp,
                    EmployeeType.Values.Permanent,
                    EmployeeType.Values.FullTime
                }
            ),
            "Should get all values"
        );
    }

    [Test]
    public void ShouldOverrideDefaultFormat()
    {
        Assert.That
        (
            TestNumericValue.Values.Test01.DisplayText,
            Is.EqualTo("TestNumericValue"),
            "Should override default display text formatting"
        );
    }

    private sealed class TestNumericValue : NumericValue, IEquatable<TestNumericValue>
    {
        public sealed class TestNumericValues : NumericValues<TestNumericValue>
        {
            public TestNumericValues()
                : base(new TestNumericValue(0, "Not Set"))
            {
                NotSet = DefaultValue;
                Test01 = Add(100, "TestNumericValue");
            }

            private TestNumericValue Add(int value, string displayText) =>
                Add(new(value, displayText));

            public TestNumericValue NotSet { get; }
            public TestNumericValue Test01 { get; }
        }

        public static readonly TestNumericValues Values = new();

        private TestNumericValue(int value, string displayText)
            : base(value, displayText, (_, v) => v)
        {
        }

        public bool Equals(TestNumericValue? other) => _Equals(other);
    }
}