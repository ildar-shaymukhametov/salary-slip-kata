public class Employee
{
    public Employee(int id, string name, decimal annualGrossSalary)
    {
        Id = id;
        Name = name;
        AnnualGrossSalary = annualGrossSalary;
    }

    public int Id { get; }
    public string Name { get; }
    public decimal AnnualGrossSalary { get; }
}