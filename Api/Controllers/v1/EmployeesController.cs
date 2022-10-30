using Core.Entities.DataTransferObjects;
using Core.Entities.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
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
    public async Task<ActionResult<ApiOkResponse<List<EmployeeDto>>>> Get()
    {
        try
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            return Ok(new ApiOkResponse<List<EmployeeDto>>(employees));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpGet("{employeeId}")]
    public async Task<ActionResult<ApiOkResponse<EmployeeDto>>> Get(string employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null) return NotFound(new ApiNotFoundResponse($"No employee found with id: {employeeId}"));
            return Ok(new ApiOkResponse<EmployeeDto>(employee));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiOkResponse<EmployeeDto>>> Post(EmployeeDto employeeDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var employee = await _employeeRepository.CreateEmployeeAsync(employeeDto);
            return Ok(new ApiOkResponse<EmployeeDto>(employee));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPut("{employeeId}")]
    public async Task<ActionResult<ApiOkResponse<EmployeeDto>>> Put(string employeeId, EmployeeDto employeeDto)
    {
        try
        {
            if (!employeeId.Equals(employeeDto.Id))
                return BadRequest(new ApiBadRequestResponse($"Id: {employeeId} is not the same as in Object"));
            var employee = await _employeeRepository.UpdateEmployeeAsync(employeeDto);
            return Ok(new ApiOkResponse<EmployeeDto>(employee));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpDelete("{employeeId}")]
    public async Task<ActionResult<ApiOkResponse<bool>>> Delete(string employeeId)
    {
        try
        {
            var checkDelete = await _employeeRepository.DeleteEmployeeAsync(employeeId);
            if (!checkDelete) return BadRequest(new ApiBadRequestResponse("Could not delete employee"));
            return Ok(new ApiOkResponse<bool>(checkDelete));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }
}