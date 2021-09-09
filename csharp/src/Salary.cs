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
        const decimal PersonalAllowance = 11000;
        if (Amount > 100000 && Amount <= 122000)
        {
            return PersonalAllowance - (Amount - 100000) / 2;
        }
        else if (Amount < 100000)
        {
            return PersonalAllowance;
        }
        else
        {
            return 0;
        }
    }

    public decimal CalculateTaxPayable()
    {
        var amount = CalculateTaxableIncome();
        var result = 0M;
        if (Amount > 11000)
        {
            var taxable = amount;
            if (taxable > 32000)
            {
                taxable = 32000;
            }
            result += taxable * 0.2M;
            amount -= taxable;
        }
        if (Amount > 43000)
        {
            var taxable = amount;
            if (taxable > 118000)
            {
                taxable = 118000;
            }
            result += taxable * 0.4M;
            amount -= taxable;
        }
        if (Amount > 150000)
        {
            result += amount * 0.45M;
        }

        return result;
    }

    public decimal CalculateNiContributions()
    {
        var amount = GetContributableIncome();
        var result = 0M;
        if (Amount > 8060)
        {
            var taxable = amount;
            if (taxable > 34940)
            {
                taxable = 34940;
            }
            result += taxable * 0.12M;
            amount -= taxable;
        }
        if (Amount > 43000)
        {
            result += amount * 0.02M;
        }

        return result;
    }

    private decimal GetContributableIncome()
    {
        return Math.Max(0, Amount - 8060);
    }
}
