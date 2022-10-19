using Core.Entities.DataTransferObjects;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{employeeId}")]
    public async Task<ActionResult<EmployeeDto>> Get(string employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null) return NotFound($"No employee found with id: {employeeId}");
            return Ok(employee);
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

    [HttpPut("{employeeId}")]
    public async Task<ActionResult<EmployeeDto>> Put(string employeeId, EmployeeDto employeeDto)
    {
        try
        {
            if (!employeeId.Equals(employeeDto.Id)) return BadRequest("Error with employeeId");
            var employee = await _employeeRepository.UpdateEmployeeAsync(employeeDto);
            return Ok(employee);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{employeeId}")]
    public async Task<ActionResult<bool>> Delete(string employeeId)
    {
        try
        {
            var checkDelete = await _employeeRepository.DeleteEmployeeAsync(employeeId);
            if (!checkDelete) return BadRequest($"Employee with id: {employeeId} could not be deleted");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}