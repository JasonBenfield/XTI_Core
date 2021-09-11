using NUnit.Framework;

namespace XTI_Core.Tests
{
    public sealed class NumericValueTest
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
        public void ShouldGetAllValues()
        {
            Assert.That
            (
                EmployeeType.Values.All(),
                Is.EquivalentTo(new[] { EmployeeType.Values.None, EmployeeType.Values.Temp, EmployeeType.Values.Permanent }),
                "Should get all values"
            );
        }
    }

}
