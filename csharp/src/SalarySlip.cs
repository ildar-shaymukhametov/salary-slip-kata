public class SalarySlip
{
    public int EmployeeId { get; }
    public string EmployeeName { get; set; }

    public SalarySlip(int employeeId, string employeeName)
    {
        EmployeeId = employeeId;
        EmployeeName = employeeName;
    }
}