using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Services;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> GetAll([FromQuery] string? search)
    {
        var employees = await _service.GetAllAsync(search);
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _service.GetByIdAsync(id);
        if (employee == null) return NotFound(new { message = $"Employee {id} not found." });
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create([FromBody] Employee employee)
    {
        try
        {
            var created = await _service.CreateAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
    {
        var success = await _service.UpdateAsync(id, employee);
        if (!success) return NotFound(new { message = $"Employee {id} not found." });
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { message = $"Employee {id} not found." });
        return NoContent();
    }
}
