using NUnit.Framework;

namespace XTI_Core.Tests
{
    internal sealed class XtiSerializerTest
    {
        [Test]
        public void ShouldDeserializeNullAsNewObject()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("null");
            Assert.That(deserialized.Name, Is.EqualTo(""));
        }

        [Test]
        public void ShouldDeserializeDepartment()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("{ \"Department\": { \"ID\": 1, \"Name\": \"IT\" } }");
            Assert.That(deserialized.Department.ID, Is.EqualTo(1));
            Assert.That(deserialized.Department.Name, Is.EqualTo("IT"));
        }

        [Test]
        public void ShouldDeserializeNullStringAsEmptyString()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("{ \"Department\": { \"ID\": 1, \"Name\": null } }");
            Assert.That(deserialized.Department.Name, Is.EqualTo(""));
        }

        [Test]
        public void ShouldDeserializeNullNumericValue()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("{ \"EmployeeType\": null }");
            Assert.That(deserialized.EmployeeType, Is.EqualTo(EmployeeType.Values.None));
        }

        [Test]
        public void ShouldDeserializeNullDepartmentAsNonNull()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("{ \"Department\": null }");
            Assert.That(deserialized.Department, Is.Not.Null);
        }
    }
}
