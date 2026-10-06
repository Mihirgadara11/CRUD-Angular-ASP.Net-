using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Repositories;

namespace EmployeeManagement.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Employee>> GetAllAsync(string? searchTerm) =>
        _repository.GetAllAsync(searchTerm);

    public Task<Employee?> GetByIdAsync(int id) =>
        _repository.GetByIdAsync(id);

    public async Task<Employee> CreateAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
            throw new ArgumentException("Name is required.");

        if (employee.Salary <= 0)
            throw new ArgumentException("Salary must be greater than zero.");

        var newId = await _repository.InsertAsync(employee);
        employee.EmployeeId = newId;
        return employee;
    }

    public Task<bool> UpdateAsync(int id, Employee employee)
    {
        employee.EmployeeId = id;
        return _repository.UpdateAsync(employee);
    }

    public Task<bool> DeleteAsync(int id) =>
        _repository.DeleteAsync(id);
}
