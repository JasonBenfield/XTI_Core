namespace XTI_Core.Tests;

internal sealed class Employee
{
    public EmployeeType EmployeeType { get; set; } = EmployeeType.Values.None;

    public string Name { get; set; } = "";

    public decimal HourlyWages { get; set; }

    private Department department = new();

    public Department Department
    {
        get => department;
        set => department = value ?? new();
    }
}

public sealed class Department
{
    public int ID { get; set; }
    public string Name { get; set; } = "";
}
