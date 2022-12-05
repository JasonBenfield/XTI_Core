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
}