public class TaxCalculator
{
    const decimal MonthsInYear = 12;

    public decimal MonthlyGrossSalary(Employee employee)
    {
        return Math.Round(employee.AnnualGrossSalary / MonthsInYear, 2);
    }

    public decimal Contributions(Employee employee)
    {
        var result = 0M;
        if (employee.AnnualGrossSalary > 8060)
        {
            result += (employee.AnnualGrossSalary - 8060) * 0.12M / MonthsInYear;
        }
        return result;
    }
}