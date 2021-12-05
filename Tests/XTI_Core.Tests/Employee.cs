namespace XTI_Core.Tests
{
    internal sealed class Employee
    {
        private EmployeeType employeeType = EmployeeType.Values.None;
        private string name = "";

        public EmployeeType EmployeeType
        {
            get => employeeType;
            set => employeeType = value ?? EmployeeType.Values.None;
        }

        public string Name
        {
            get => name;
            set => name = value ?? "";
        }

        public decimal HourlyWages { get; set; }
    }
}
