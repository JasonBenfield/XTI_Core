using NUnit.Framework;
using System.ComponentModel;

namespace XTI_Core.Tests;

public sealed class SerializeNumericValueTest
{
    [Test]
    public void ShouldDeserializeNumericValue()
    {
        var employee = new Employee
        {
            Name = "Xartogg",
            EmployeeType = EmployeeType.Values.Permanent,
            HourlyWages = 12.34M
        };
        var serialized = XtiSerializer.Serialize(employee);
        var deserialized = XtiSerializer.Deserialize<Employee>(serialized);
        Assert.That(deserialized.EmployeeType, Is.EqualTo(EmployeeType.Values.Permanent));
        Assert.That(deserialized.Name, Is.EqualTo("Xartogg"));
        Assert.That(deserialized.HourlyWages, Is.EqualTo(12.34));
    }

    [Test]
    public void ShouldDeserializeNumericValueFromNumber()
    {
        var deserialized = XtiSerializer.Deserialize<Employee>("{ \"EmployeeType\": 10 }");
        Assert.That(deserialized.EmployeeType, Is.EqualTo(EmployeeType.Values.Temp));
    }

    [Test]
    public void ShouldDeserializeNumericValueFromString()
    {
        var deserialized = XtiSerializer.Deserialize<Employee>("{ \"EmployeeType\": \"Permanent\" }");
        Assert.That(deserialized.EmployeeType, Is.EqualTo(EmployeeType.Values.Permanent));
    }

    [Test]
    public void ShouldDeserializeNullStringAsEmptyString()
    {
        var deserialized = XtiSerializer.Deserialize<Employee>("{ \"EmployeeType\": \"Permanent\", \"Name\": null }");
        Assert.That(deserialized.Name, Is.EqualTo(""));
    }

    [Test]
    public void ShouldConvertNumericValueFromInt()
    {
        var original = EmployeeType.Values.Temp;
        var converted = new NumericValueTypeConverter<EmployeeType>().ConvertFrom(original.Value);
        Assert.That(converted, Is.EqualTo(original));
    }

    [Test]
    public void ShouldConvertNumericValueFromString()
    {
        var original = EmployeeType.Values.Permanent;
        var converted = new NumericValueTypeConverter<EmployeeType>().ConvertFrom(original.DisplayText);
        Assert.That(converted, Is.EqualTo(original));
    }

    [Test]
    public void ShouldConvertAppEventSeverityFromString()
    {
        var original = AppEventSeverity.Values.CriticalError;
        var converted = TypeDescriptor.GetConverter(typeof(AppEventSeverity)).ConvertFrom("Critical Error");
        Assert.That(converted, Is.EqualTo(original));
    }
}