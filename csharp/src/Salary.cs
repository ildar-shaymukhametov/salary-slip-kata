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
        var result = 0M;
        if (Amount <= 100000)
        {
            result = 11000;
        }
        if (Amount > 100000 && Amount <= 122000)
        {
            result = 11000 - (Amount - 100000) / 2;
        }

        return result;
    }

    public decimal CalculateTaxPayable()
    {
        return new ITaxBand[]
        {
            new BasicTaxBand(),
            new HigherTaxBand(),
            new AdditionalTaxBand()
        }
        .Sum(x => x.Calculate(this));
    }

    public decimal CalculateNiContributions()
    {
        var result = 0M;
        if (Amount > 43000)
        {
            result += (Amount - 43000) * 0.02M;
        }
        if (Amount > 8060)
        {
            result += Math.Min(Amount - 8060, 43000 - 8060) * 0.12M;
        }

        return result;
    }
}

public interface ITaxBand
{
    decimal Calculate(Salary salary);
}

public abstract class TaxBand : ITaxBand
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

public class BasicTaxBand : TaxBand
{
    protected override decimal LowerLimit => 11000;
    protected override decimal UpperLimit => 43000;
    protected override decimal Rate => 0.2M;
}

public class HigherTaxBand : TaxBand
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

public class AdditionalTaxBand : TaxBand
{
    protected override decimal LowerLimit => 150000;
    protected override decimal UpperLimit => decimal.MaxValue;
    protected override decimal Rate => 0.45M;
}
