public class Salary
{
    public decimal Amount { get; }

    public Salary(decimal amount)
    {
        this.Amount = amount;
    }

    public decimal CalculateTaxableIncome()
    {
        return Math.Max(0, Amount - CalculateTaxFreeAllowance());
    }

    public decimal CalculateTaxFreeAllowance()
    {
        return GetPersonalAllowance().Get();
    }

    private IPersonalAllowance GetPersonalAllowance()
    {
        IPersonalAllowance result;
        if (Amount > 100000)
        {
            result = new ReducedPersonalAllowance(new DefaultPersonalAllowance(), this);
        }
        else
        {
            result = new DefaultPersonalAllowance();
        }

        return result;
    }

    public decimal CalculateTaxPayable()
    {
        var contributions = new List<IContributions>();
        if (Amount > 11000)
        {
            contributions.Add(new BasicRate());
        }
        if (Amount > 43000)
        {
            contributions.Add(new HigherRate());
        }
        if (Amount > 150000)
        {
            contributions.Add(new AdditionalRate());
        }

        return contributions.Sum(x => x.Calculate(this));
    }

    public decimal CalculateNiContributions()
    {
        var contributions = new List<IContributions>();
        if (Amount > 8060)
        {
            contributions.Add(new BasicContributions());
        }
        if (Amount > 43000)
        {
            contributions.Add(new HigherContributions());
        }

        return contributions.Sum(x => x.Calculate(this));
    }
}

public interface IPersonalAllowance
{
    decimal Get();
}

public class DefaultPersonalAllowance : IPersonalAllowance
{
    public decimal Get() => 11000;
}

public class ReducedPersonalAllowance : IPersonalAllowance
{
    private readonly IPersonalAllowance defaultAllowance;
    private readonly Salary salary;

    public ReducedPersonalAllowance(IPersonalAllowance defaultAllowance, Salary salary)
    {
        this.defaultAllowance = defaultAllowance;
        this.salary = salary;
    }

    public decimal Get()
    {
        var result = defaultAllowance.Get() - (salary.Amount - 100000) / 2;
        return Math.Max(0, result);
    }
}

public interface IContributions
{
    decimal Calculate(Salary salary);
}

public class BasicContributions : IContributions
{
    private const decimal Rate = 0.12M;
    private const decimal Max = 34940;
    private const decimal ContributionFreeAllowance = 8060;

    public decimal Calculate(Salary salary)
    {
        return Math.Min(salary.Amount - ContributionFreeAllowance, Max) * Rate;
    }
}

public class HigherContributions : IContributions
{
    private const decimal Rate = 0.02M;
    private const decimal Max = 43000;

    public decimal Calculate(Salary salary)
    {
        return (salary.Amount - Max) * Rate;
    }
}

public class BasicRate : IContributions
{
    private const decimal Rate = 0.2M;
    private const decimal Max = 32000;

    public decimal Calculate(Salary salary)
    {
        return Math.Min(salary.CalculateTaxableIncome(), Max) * Rate;
    }
}

public class HigherRate : IContributions
{
    private const decimal Rate = 0.4M;
    private const decimal Max = 118000;
    private const int AlreadyTaxed = 32000;

    public decimal Calculate(Salary salary)
    {
        return Math.Min(salary.CalculateTaxableIncome() - AlreadyTaxed, Max) * Rate;
    }
}

public class AdditionalRate : IContributions
{
    private const decimal Rate = 0.45M;
    private const int AlreadyTaxed = 150000;

    public decimal Calculate(Salary salary)
    {
        return (salary.CalculateTaxableIncome() - AlreadyTaxed) * Rate;
    }
}
