using System.Globalization;

public class SalarySlipGenerator
{
    public SalarySlip GenerateFor(Employee employee)
    {
        var salary = new Salary(employee.AnnualGrossSalary);
        return new SalarySlip(
            employee.Id,
            employee.Name,
            ToCurrency(employee.AnnualGrossSalary / 12),
            ToCurrency(salary.CalculateNiContributions() / 12),
            ToCurrency(salary.CalculateTaxFreeAllowance() / 12),
            ToCurrency(salary.CalculateTaxableIncome() / 12),
            ToCurrency(salary.CalculateTaxPayable() / 12)
        );
    }

    private string ToCurrency(decimal amount)
    {
        return amount.ToString("C", new CultureInfo("en-GB"));
    }
}
