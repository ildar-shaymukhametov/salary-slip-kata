using System.Globalization;

public class SalarySlipGenerator
{
    public SalarySlip GenerateFor(Employee employee)
    {
        return new SalarySlip(
            employee.Id,
            employee.Name,
            ToCurrency(employee.AnnualGrossSalary / 12),
            ToCurrency(CalculateNiContributions(employee.AnnualGrossSalary) / 12),
            ToCurrency(CalculateTaxFreeAllowance(employee.AnnualGrossSalary) / 12),
            ToCurrency(CalculateTaxableIncome(employee.AnnualGrossSalary) / 12),
            ToCurrency(CalculateTaxPayable(employee.AnnualGrossSalary) / 12)
        );
    }

    private decimal CalculateTaxPayable(decimal annualSalary)
    {
        var result = 0M;
        if (annualSalary > 150000)
        {
            var amount = annualSalary - 150000;
            result += (amount) * 0.45M;
        }
        if (annualSalary > 43000)
        {
            var amount = Math.Min(annualSalary - 43000, 150000 - 43000) + (11000 - CalculateTaxFreeAllowance(annualSalary));
            result += amount * 0.4M;
        }
        if (annualSalary > 11000)
        {
            var amount = Math.Min(annualSalary - 11000, 43000 - 11000);
            result += amount * 0.2M;
        }

        return result;
    }

    private decimal CalculateTaxableIncome(decimal annualSalary)
    {
        return Math.Max(0, annualSalary - CalculateTaxFreeAllowance(annualSalary));
    }

    private decimal CalculateTaxFreeAllowance(decimal annualSalary)
    {
        var result = 0M;
        if (annualSalary <= 100000)
        {
            result = 11000;
        }
        if (annualSalary > 100000 && annualSalary <= 122000)
        {
            result = 11000 - (annualSalary - 100000) / 2;
        }

        return result;
    }

    private decimal CalculateNiContributions(decimal annualSalary)
    {
        var result = 0M;
        if (annualSalary > 43000)
        {
            result += (annualSalary - 43000) * 0.02M;
        }
        if (annualSalary > 8060)
        {
            result += Math.Min(annualSalary - 8060, 43000 - 8060) * 0.12M;
        }
        return result;
    }

    private string ToCurrency(decimal amount)
    {
        return amount.ToString("C", new CultureInfo("en-GB"));
    }
}
