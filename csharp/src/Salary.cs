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

public class BasicTaxBand : ITaxBand
{
    public decimal Calculate(Salary salary)
    {
        var taxable = Math.Min(salary.Amount - 11000, 43000 - 11000);
        return Math.Max(0, taxable) * 0.2M;
    }
}

public class HigherTaxBand : ITaxBand
{
    public decimal Calculate(Salary salary)
    {
        var taxable = Math.Min(salary.Amount - 43000, 150000 - 43000) + (11000 - salary.CalculateTaxFreeAllowance());
        return Math.Max(0, taxable) * 0.4M;
    }
}

public class AdditionalTaxBand : ITaxBand
{
    public decimal Calculate(Salary salary)
    {
        var taxable = salary.Amount - 150000;
        return Math.Max(0, taxable) * 0.45M;
    }
}
