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

        private static Employee CreateEmployee(decimal annualGrossSalary)
        {
            return new Employee(1, "Foo", annualGrossSalary);
        }
    }
}
