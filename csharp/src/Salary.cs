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
        return new IContributions[]
        {
            new BasicTaxBand(),
            new HigherTaxBand(),
            new AdditionalTaxBand()
        }
        .Sum(x => x.Calculate(this));
    }

    public decimal CalculateNiContributions()
    {
        return new IContributions[]
        {
            new BasicContributions(),
            new HigherContributions(),
        }
        .Sum(x => x.Calculate(this));
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

public abstract class Contributions : IContributions
{
    protected abstract decimal LowerLimit { get; }
    protected abstract decimal UpperLimit { get; }
    protected abstract decimal Rate { get; }

    public virtual decimal Calculate(Salary salary)
    {
        var maxAmount = UpperLimit - LowerLimit;
        var currentAmount = salary.Amount - LowerLimit;
        var amount = Math.Min(currentAmount, maxAmount);
        return Math.Max(amount * Rate, 0);
    }
}

public class BasicContributions : Contributions
{
    protected override decimal LowerLimit => 8060;
    protected override decimal UpperLimit => 43000;
    protected override decimal Rate => 0.12M;
}

public class HigherContributions : Contributions
{
    protected override decimal LowerLimit => 43000;
    protected override decimal UpperLimit => decimal.MaxValue;
    protected override decimal Rate => 0.02M;
}

public class BasicTaxBand : Contributions
{
    protected override decimal LowerLimit => 11000;
    protected override decimal UpperLimit => 43000;
    protected override decimal Rate => 0.2M;
}

public class HigherTaxBand : Contributions
{
    protected override decimal LowerLimit => 43000;
    protected override decimal UpperLimit => 150000;
    protected override decimal Rate => 0.4M;

    public override decimal Calculate(Salary salary)
    {
        var extra = (11000 - salary.CalculateTaxFreeAllowance()) * Rate;
        return extra + base.Calculate(salary);
    }
}

public class AdditionalTaxBand : Contributions
{
    protected override decimal LowerLimit => 150000;
    protected override decimal UpperLimit => decimal.MaxValue;
    protected override decimal Rate => 0.45M;
}
