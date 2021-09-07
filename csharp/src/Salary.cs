public class Salary
{
    private readonly decimal amount;

    public Salary(decimal amount)
    {
        this.amount = amount;
    }

    public decimal CalculateTaxableIncome()
    {
        return Math.Max(0, amount - CalculateTaxFreeAllowance());
    }

    public decimal CalculateTaxFreeAllowance()
    {
        var result = 0M;
        if (amount <= 100000)
        {
            result = 11000;
        }
        if (amount > 100000 && amount <= 122000)
        {
            result = 11000 - (amount - 100000) / 2;
        }

        return result;
    }

    public decimal CalculateTaxPayable()
    {
        var result = 0M;
        if (amount > 150000)
        {
            result += GetAdditionalTax(amount);
        }
        if (amount > 43000)
        {
            result += GetHigherTax(amount);
        }
        if (amount > 11000)
        {
            result += GetBasicTax(amount);
        }

        return result;
    }

    private static decimal GetBasicTax(decimal amount)
    {
        var taxable = (Math.Min(amount - 11000, 43000 - 11000));
        return taxable * 0.2M;
    }

    private decimal GetHigherTax(decimal amount)
    {
        var taxable = Math.Min(amount - 43000, 150000 - 43000) + (11000 - CalculateTaxFreeAllowance());
        return taxable * 0.4M;
    }

    private static decimal GetAdditionalTax(decimal amount)
    {
        var taxable = amount - 150000;
        return taxable * 0.45M;
    }

    public decimal CalculateNiContributions()
    {
        var result = 0M;
        if (amount > 43000)
        {
            result += (amount - 43000) * 0.02M;
        }
        if (amount > 8060)
        {
            result += Math.Min(amount - 8060, 43000 - 8060) * 0.12M;
        }

        return result;
    }
}