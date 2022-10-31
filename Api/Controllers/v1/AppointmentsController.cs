using Core.Entities.DataTransferObjects;
using Core.Entities.Responses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(IAppointmentRepository appointmentRepository, ILogger<AppointmentsController> logger)
    {
        _appointmentRepository =
            appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{appointmentId}")]
    public async Task<ActionResult<ApiOkResponse<AppointmentDto>>> Get(string appointmentId)
    {
        try
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
                return NotFound(new ApiNotFoundResponse($"Appointment with id: {appointmentId} is not found"));
            return Ok(new ApiOkResponse<AppointmentDto>(appointment));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpGet("Customers/{customerId}")]
    public async Task<ActionResult<ApiOkResponse<List<AppointmentDto>>>> GetCustomerAppointments(string customerId)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAppointmentsByCustomerIdAsync(customerId);
            return Ok(new ApiOkResponse<List<AppointmentDto>>(appointments));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpGet("Employees/{employeeId}")]
    public async Task<ActionResult<ApiOkResponse<List<AppointmentDto>>>> GetEmployeeAppointments(string employeeId)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAppointmentsByEmployeeIdAsync(employeeId);
            return Ok(new ApiOkResponse<List<AppointmentDto>>(appointments));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiOkResponse<AppointmentDto>>> Post(AppointmentDto appointmentDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var appointment = await _appointmentRepository.CreateAppointmentAsync(appointmentDto);
            return Ok(new ApiOkResponse<AppointmentDto>(appointment));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpPut("{appointmentId}")]
    public async Task<ActionResult<ApiOkResponse<AppointmentDto>>> Put(string appointmentId,
        AppointmentDto appointmentDto)
    {
        try
        {
            if (!appointmentId.Equals(appointmentDto.Id))
                return BadRequest(new ApiBadRequestResponse($"Id: {appointmentId} is not the same as in Object"));
            var appointment = await _appointmentRepository.UpdateAppointmentAsync(appointmentDto);
            return Ok(new ApiOkResponse<AppointmentDto>(appointment));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiInternalServerErrorResponse(e.Message));
        }
    }

    [HttpDelete("{appointmentId}")]
    public async Task<ActionResult<ApiOkResponse<bool>>> Delete(string appointmentId)
    {
        try
        {
            var checkDelete = await _appointmentRepository.DeleteAppointmentByIdAsync(appointmentId);
            if (!checkDelete) return BadRequest(new ApiBadRequestResponse("Could not delete appointment"));
            return Ok(new ApiOkResponse<bool>(checkDelete));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}