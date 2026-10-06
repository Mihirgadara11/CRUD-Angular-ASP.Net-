using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");
    }

    public async Task<List<Employee>> GetAllAsync(string? searchTerm)
    {
        var employees = new List<Employee>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("PR_Employee_GetAll", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 150).Value =
            (object?)searchTerm ?? DBNull.Value;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            employees.Add(MapEmployee(reader));
        }

        return employees;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("PR_Employee_GetById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapEmployee(reader);
        }

        return null;
    }

    public async Task<int> InsertAsync(Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("PR_Employee_Insert", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = employee.Name;
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = employee.Email;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = employee.Salary;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = employee.DepartmentId;

        var outputParam = new SqlParameter("@NewEmployeeId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParam);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();

        return (int)outputParam.Value;
    }

    public async Task<bool> UpdateAsync(Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("PR_Employee_Update", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employee.EmployeeId;
        command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = employee.Name;
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = employee.Email;
        command.Parameters.Add("@Salary", SqlDbType.Decimal).Value = employee.Salary;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = employee.DepartmentId;

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("PR_Employee_Delete", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();

        return rowsAffected > 0;
    }

    private static Employee MapEmployee(SqlDataReader reader)
    {
        return new Employee
        {
            EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
            DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
        };
    }
}
