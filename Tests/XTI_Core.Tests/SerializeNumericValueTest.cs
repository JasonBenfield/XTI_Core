using NUnit.Framework;

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

}