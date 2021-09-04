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

        private static Employee CreateEmployee(decimal annualGrossSalary)
        {
            return new Employee(1, "Foo", annualGrossSalary);
        }
    }
}
