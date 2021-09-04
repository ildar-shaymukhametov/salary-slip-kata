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
        var higher = Subtract(employee.AnnualGrossSalary, 43000);
        result += higher * 0.02M / MonthsInYear;

        var basic = Subtract(employee.AnnualGrossSalary - higher, 8060);
        result += basic * 0.12M / MonthsInYear;

        return Math.Round(result, 2);
    }

    private static decimal Subtract(decimal left, decimal right)
    {
        var result = left - right;
        return result >= 0 ? result : 0;
    }
}