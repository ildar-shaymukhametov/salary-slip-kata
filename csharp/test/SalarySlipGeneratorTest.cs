using Xunit;

namespace test
{
    public class SalarySlipGeneratorTest
    {
        [Fact]
        public void Salary_5000()
        {
            var result = new SalarySlipGenerator().GenerateFor(new Employee(1, "Foo", 5000));
            Assert.Equal(1, result.EmployeeId);
            Assert.Equal("Foo", result.EmployeeName);
            Assert.Equal("£416.67", result.GrossSalary);
        }

        [Fact]
        public void Salary_9060()
        {
            const decimal AnnualGrossSalary = 9060;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£755.00", result.GrossSalary);
            Assert.Equal("£10.00", result.NiContributions);
        }

        [Fact]
        public void Salary_12000()
        {
            const decimal AnnualGrossSalary = 12000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£1,000.00", result.GrossSalary);
            Assert.Equal("£39.40", result.NiContributions);
            Assert.Equal("£916.67", result.TaxFreeAllowance);
            Assert.Equal("£83.33", result.TaxableIncome);
            Assert.Equal("£16.67", result.TaxPayable);
        }

        [Fact]
        public void Salary_45000()
        {
            const decimal AnnualGrossSalary = 45000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£3,750.00", result.GrossSalary);
            Assert.Equal("£352.73", result.NiContributions);
            Assert.Equal("£916.67", result.TaxFreeAllowance);
            Assert.Equal("£2,833.33", result.TaxableIncome);
            Assert.Equal("£600.00", result.TaxPayable);
        }

        [Fact]
        public void Salary_101000()
        {
            const decimal AnnualGrossSalary = 101000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£8,416.67", result.GrossSalary);
            Assert.Equal("£446.07", result.NiContributions);
            Assert.Equal("£875.00", result.TaxFreeAllowance);
            Assert.Equal("£7,541.67", result.TaxableIncome);
            Assert.Equal("£2,483.33", result.TaxPayable);
        }

        [Fact]
        public void Salary_111000()
        {
            const decimal AnnualGrossSalary = 111000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£9,250.00", result.GrossSalary);
            Assert.Equal("£462.73", result.NiContributions);
            Assert.Equal("£458.33", result.TaxFreeAllowance);
            Assert.Equal("£8,791.67", result.TaxableIncome);
            Assert.Equal("£2,983.33", result.TaxPayable);
        }

        [Fact]
        public void Salary_122000()
        {
            const decimal AnnualGrossSalary = 122000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£10,166.67", result.GrossSalary);
            Assert.Equal("£481.07", result.NiContributions);
            Assert.Equal("£0.00", result.TaxFreeAllowance);
            Assert.Equal("£10,166.67", result.TaxableIncome);
            Assert.Equal("£3,533.33", result.TaxPayable);
        }

        [Fact]
        public void Salary_150000()
        {
            const decimal AnnualGrossSalary = 150000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£12,500.00", result.GrossSalary);
            Assert.Equal("£527.73", result.NiContributions);
            Assert.Equal("£0.00", result.TaxFreeAllowance);
            Assert.Equal("£12,500.00", result.TaxableIncome);
            Assert.Equal("£4,466.67", result.TaxPayable);
        }

        [Fact]
        public void Salary_160000()
        {
            const decimal AnnualGrossSalary = 160000;
            var result = new SalarySlipGenerator().GenerateFor(CreateEmployee(AnnualGrossSalary));
            Assert.Equal("£13,333.33", result.GrossSalary);
            Assert.Equal("£544.40", result.NiContributions);
            Assert.Equal("£0.00", result.TaxFreeAllowance);
            Assert.Equal("£13,333.33", result.TaxableIncome);
            Assert.Equal("£4,841.67", result.TaxPayable);
        }

        private static Employee CreateEmployee(decimal annualGrossSalary)
        {
            return new Employee(1, "Foo", annualGrossSalary);
        }
    }
}
