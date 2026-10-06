namespace EmployeeManagement.Api.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime CreatedDate { get; set; }
}
