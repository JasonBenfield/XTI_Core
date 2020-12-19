using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTI_Core.Tests
{
    public sealed class EmployeeType : NumericValue
    {
        public sealed class EmployeeTypes : NumericValues<EmployeeType>
        {
            internal EmployeeTypes() : base(new EmployeeType(0, "None"))
            {
                None = DefaultValue;
                Temp = Add(new EmployeeType(10, "Temp"));
                Permanent = Add(new EmployeeType(15, "Permanent"));
            }
            public EmployeeType None { get; }
            public EmployeeType Temp { get; }
            public EmployeeType Permanent { get; }
        }

        public static readonly EmployeeTypes Values = new EmployeeTypes();

        private EmployeeType(int value, string displayText) : base(value, displayText)
        {
        }
    }

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
