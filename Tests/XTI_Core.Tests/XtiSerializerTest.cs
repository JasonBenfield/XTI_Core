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
        public void ShouldDeserializeNullNumericValueAsDefault()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("{ \"EmployeeType\": null }");
            Assert.That(deserialized.EmployeeType, Is.EqualTo(EmployeeType.Values.GetDefault()));
        }

        [Test]
        public void ShouldDeserializeNullDepartmentAsNonNull()
        {
            var deserialized = XtiSerializer.Deserialize<Employee>("{ \"Department\": null }");
            Assert.That(deserialized.Department, Is.Not.Null);
        }

        [Test]
        public void ShouldDeserializeRecord()
        {
            var original = new TestRecord(1, EmployeeType.Values.Permanent);
            var serialized = XtiSerializer.Serialize(original);
            var deserialized = XtiSerializer.Deserialize(serialized, () => new TestRecord(0, EmployeeType.Values.None));
            Assert.That(deserialized, Is.EqualTo(original));
        }

        public sealed record TestRecord(int ID, EmployeeType Type);
    }
}
