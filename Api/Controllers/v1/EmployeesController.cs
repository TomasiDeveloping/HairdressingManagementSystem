using Core.Entities.DataTransferObjects;
using Core.Helpers.Services;
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
            var errorResponse =
                ErrorService.CreateError("Error in Get Employee", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
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
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Error in Get Employee", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
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
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Could not create new employee", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPut("{employeeId}")]
    public async Task<ActionResult<EmployeeDto>> Put(string employeeId, EmployeeDto employeeDto)
    {
        try
        {
            if (!employeeId.Equals(employeeDto.Id))
                throw new ArgumentException($"Id: {employeeId} is not the same as in Object");
            var employee = await _employeeRepository.UpdateEmployeeAsync(employeeDto);
            return Ok(employee);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Could not update employee", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpDelete("{employeeId}")]
    public async Task<ActionResult<bool>> Delete(string employeeId)
    {
        try
        {
            var checkDelete = await _employeeRepository.DeleteEmployeeAsync(employeeId);
            if (!checkDelete) throw new Exception();
                return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse =
                ErrorService.CreateError("Could not delete employee", StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }
}