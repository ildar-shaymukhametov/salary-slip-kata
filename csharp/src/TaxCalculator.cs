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

    public decimal TaxableIncome(Employee employee)
    {
        var taxFreeAllowance = GetTaxFreeAllowance(employee.AnnualGrossSalary).Calculate();
        if (employee.AnnualGrossSalary <= taxFreeAllowance)
        {
            return 0;
        }
        return Math.Round((employee.AnnualGrossSalary - taxFreeAllowance) / MonthsInYear, 2);
    }

    public decimal TaxPayable(Employee employee)
    {
        var result = GetRateBands().Sum(x => x.Calculate(new Salary(employee.AnnualGrossSalary)));
        return Math.Round(result / MonthsInYear, 2);
    }

    private static List<IRateBand> GetRateBands()
    {
        return new List<IRateBand>
        {
            new BasicRateBand(),
            new HigherRateBand(),
            new AdditionalRateBand()
        };
    }
}

public interface IRateBand
{
    decimal Calculate(Salary salary);
}

public abstract class RateBand : IRateBand
{
    protected abstract decimal LowerLimit { get; }
    protected abstract decimal? HigherLimit { get; }
    protected abstract decimal Rate { get; }

    public virtual decimal Calculate(Salary salary)
    {
        var result = 0M;
        if (salary.Value > HigherLimit)
        {
            result = HigherLimit.Value - LowerLimit;
        }
        else if (salary.Value > LowerLimit)
        {
            result = salary.Value - LowerLimit;
        }

        return result * Rate;
    }
}

public class BasicRateBand : RateBand
{
    protected override decimal LowerLimit => 11000;
    protected override decimal? HigherLimit => 43000;
    protected override decimal Rate => 0.2M;
}

public class HigherRateBand : RateBand
{
    protected override decimal LowerLimit => 43000;
    protected override decimal? HigherLimit => 150000;
    protected override decimal Rate => 0.4M;

    public override decimal Calculate(Salary salary)
    {
        return base.Calculate(salary) + salary.GetPersonalAllowanceExcess() * Rate;
    }
}

public class AdditionalRateBand : RateBand
{
    protected override decimal LowerLimit => 150000;
    protected override decimal? HigherLimit => null;
    protected override decimal Rate => 0.45M;
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

public class Salary
{
    public decimal Value { get; }

    public Salary(decimal Value)
    {
        this.Value = Value;
    }

    public decimal GetPersonalAllowanceExcess()
    {
        if (Value <= 100000)
        {
            return 0;
        }
        var result = (Value - 100000) / 2;
        return result < 11000 ? result : 11000;
    }
}