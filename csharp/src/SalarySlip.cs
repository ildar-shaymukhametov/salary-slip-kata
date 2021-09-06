public class SalarySlip
{
    public int EmployeeId { get; }
    public string EmployeeName { get; set; }
    public string GrossSalary { get; set; }
    public string NiContributions { get; set; }
    public string TaxFreeAllowance { get; set; }
    public string TaxableIncome { get; set; }
    public string TaxPayable { get; set; }

    public SalarySlip(int employeeId, string employeeName, string grossSalary, string niContributions, string taxFreeAllowance, string taxableIncome, string taxPayable)
    {
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        GrossSalary = grossSalary;
        NiContributions = niContributions;
        TaxFreeAllowance = taxFreeAllowance;
        TaxableIncome = taxableIncome;
        TaxPayable = taxPayable;
    }
}