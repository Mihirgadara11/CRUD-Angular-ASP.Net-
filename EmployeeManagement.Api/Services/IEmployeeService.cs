using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync(string? searchTerm);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task<bool> UpdateAsync(int id, Employee employee);
    Task<bool> DeleteAsync(int id);
}
