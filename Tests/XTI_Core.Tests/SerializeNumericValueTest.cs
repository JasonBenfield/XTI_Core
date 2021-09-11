using NUnit.Framework;
using System.Text.Json;

namespace XTI_Core.Tests
{
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
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new NumericValueJsonConverter());
            var serialized = JsonSerializer.Serialize(employee, jsonOptions);
            var deserialized = JsonSerializer.Deserialize<Employee>(serialized, jsonOptions);
            Assert.That(deserialized.EmployeeType, Is.EqualTo(EmployeeType.Values.Permanent));
            Assert.That(deserialized.Name, Is.EqualTo("Xartogg"));
            Assert.That(deserialized.HourlyWages, Is.EqualTo(12.34));
        }

        private sealed class Employee
        {
            public EmployeeType EmployeeType { get; set; }
            public string Name { get; set; }
            public decimal HourlyWages { get; set; }
        }
    }
}
