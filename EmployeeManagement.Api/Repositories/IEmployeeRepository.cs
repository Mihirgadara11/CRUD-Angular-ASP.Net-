using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync(string? searchTerm);
    Task<Employee?> GetByIdAsync(int id);
    Task<int> InsertAsync(Employee employee);
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
}
