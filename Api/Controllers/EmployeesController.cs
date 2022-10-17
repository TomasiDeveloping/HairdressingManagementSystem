using Core.Entities.DataTransferObjects;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> Get()
    {
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            if (!employees.Any()) return NoContent();
            return Ok(employees);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Post(EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeRepository.CreateEmployeeAsync(employeeDto);
            return Created("", employee);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}