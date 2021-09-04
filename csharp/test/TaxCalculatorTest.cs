using Xunit;

namespace test
{
    public class TaxCalculatorTest
    {
        [Fact]
        public void Gross_salary()
        {
            const decimal annualGrossSalary = 5000M;
            const decimal expected = 416.67M;
            var actual = new TaxCalculator().MonthlyGrossSalary(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5000)]
        [InlineData(8060)]
        public void No_contributions(decimal annualGrossSalary)
        {
            const decimal expected = 0M;
            var actual = new TaxCalculator().Contributions(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(9060, 10)]
        [InlineData(40000, 319.4)]
        [InlineData(43000, 349.4)]
        public void Basic_contributions(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().Contributions(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(45000, 352.73)]
        [InlineData(50000, 361.07)]
        [InlineData(60000, 377.73)]
        public void Higher_contributions(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().Contributions(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5000, 916.67)]
        [InlineData(100000, 916.67)]
        public void Tax_free_allowance(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().TaxFreeAllowance(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(105500, 687.50)]
        [InlineData(111000, 458.33)]
        public void Reduced_tax_free_allowance(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().TaxFreeAllowance(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(122000, 0)]
        [InlineData(160000, 0)]
        public void No_tax_free_allowance(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().TaxFreeAllowance(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5000, 0)]
        [InlineData(11000, 0)]
        public void No_taxable_income(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().TaxableIncome(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(12000, 83.33)]
        [InlineData(100000, 7416.67)]
        [InlineData(160000, 13333.33)]
        public void Taxable_income(decimal annualGrossSalary, decimal expected)
        {
            var actual = new TaxCalculator().TaxableIncome(CreateEmployee(annualGrossSalary));
            Assert.Equal(expected, actual);
        }

        private static Employee CreateEmployee(decimal annualGrossSalary)
        {
            return new Employee(1, "Foo", annualGrossSalary);
        }
    }
}
