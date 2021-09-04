public class SalarySlipGenerator
{
    public SalarySlip GenerateFor(Employee employee)
    {
        return new SalarySlip(employee.Id, employee.Name);
    }
}
