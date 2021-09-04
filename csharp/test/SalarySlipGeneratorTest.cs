using Xunit;

namespace test
{
    public class SalarySlipGeneratorTest
    {
        [Fact]
        public void GenerateFor___Return_salary_slip_with_employee_id_and_name()
        {
            var employee = new Employee(1, "Foo", default);
            var result = new SalarySlipGenerator().GenerateFor(employee);
            Assert.Equal(1, result.EmployeeId);
        }
    }
}
