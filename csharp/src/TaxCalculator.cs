public class TaxCalculator
{
    const decimal MonthsInYear = 12;

    public decimal MonthlyGrossSalary(Employee employee)
    {
        return Math.Round(employee.AnnualGrossSalary / MonthsInYear, 2);
    }

    public decimal Contributions(Employee employee)
    {
        var result = GetContributions().Sum(x => x.Calculate(employee.AnnualGrossSalary));
        return Math.Round(result / MonthsInYear, 2);
    }

    private static List<IContributions> GetContributions()
    {
        return new List<IContributions>
        {
            new BasicContributions(),
            new HigherContributions()
        };
    }

    public decimal TaxFreeAllowance(Employee employee)
    {
        return Math.Round(GetTaxFreeAllowance(employee.AnnualGrossSalary).Calculate() / MonthsInYear, 2);
    }

    private ITaxFreeAllowance GetTaxFreeAllowance(decimal annualGrossSalary)
    {
        ITaxFreeAllowance result;
        switch (annualGrossSalary)
        {
            case <= 100000:
                result = new StandardTaxFreeAllowance();
                break;
            case <= 122000:
                result = new ReducedTaxFreeAllowance(annualGrossSalary);
                break;
            default:
                result = new NoTaxFreeAllowance();
                break;
        }

        return result;
    }
}

public interface ITaxFreeAllowance
{
    decimal Calculate();
}

public class NoTaxFreeAllowance : ITaxFreeAllowance
{
    public decimal Calculate()
    {
        return 0;
    }
}

public class StandardTaxFreeAllowance : ITaxFreeAllowance
{
    public decimal Calculate()
    {
        return 11000;
    }
}

public class ReducedTaxFreeAllowance : ITaxFreeAllowance
{
    private readonly decimal salary;

    public ReducedTaxFreeAllowance(decimal salary)
    {
        this.salary = salary;
    }

    public decimal Calculate()
    {
        return 11000 - (salary - 100000) / 2;
    }
}

public interface IContributions
{
    decimal Calculate(decimal salary);
}

public class BasicContributions : IContributions
{
    const decimal LowerThreshold = 8060M;
    const decimal UpperThreshold = 43000M;
    const decimal Rate = 0.12M;

    public decimal Calculate(decimal salary)
    {
        if (salary <= LowerThreshold)
        {
            return 0;
        }

        return GetTaxableAmount(salary) * Rate;
    }

    private static decimal GetTaxableAmount(decimal salary)
    {
        var higher = salary - UpperThreshold;
        higher = higher < 0 ? 0 : higher;

        return salary - higher - LowerThreshold;
    }
}

public class HigherContributions : IContributions
{
    const decimal Threshold = 43000M;
    const decimal Rate = 0.02M;

    public decimal Calculate(decimal salary)
    {
        if (salary <= Threshold)
        {
            return 0;
        }

        return (salary - Threshold) * Rate;
    }
}