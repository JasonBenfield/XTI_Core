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
    }
}
