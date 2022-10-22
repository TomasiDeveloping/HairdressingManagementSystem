using Core.Entities.DataTransferObjects;
using Core.Helpers.Services;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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
    public async Task<ActionResult<AppointmentDto>> Get(string appointmentId)
    {
        try
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return NotFound($"No appointment found with id: {appointmentId}");
            return Ok(appointment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Error in get appointment by id",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpGet("Customers/{customerId}")]
    public async Task<ActionResult<List<AppointmentDto>>> GetCustomerAppointments(string customerId)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAppointmentsByCustomerIdAsync(customerId);
            if (!appointments.Any()) return NoContent();
            return Ok(appointments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError($"Error in get appointments for customer with id: {customerId}",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpGet("Employees/{employeeId}")]
    public async Task<ActionResult<List<AppointmentDto>>> GetEmployeeAppointments(string employeeId)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAppointmentsByEmployeeIdAsync(employeeId);
            if (!appointments.Any()) return NoContent();
            return Ok(appointments);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError(
                $"Error in get appointments for employee with id: {employeeId}", StatusCodes.Status400BadRequest,
                e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Post(AppointmentDto appointmentDto)
    {
        try
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            var appointment = await _appointmentRepository.CreateAppointmentAsync(appointmentDto);
            return Ok(appointment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not create new appointment",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpPut("{appointmentId}")]
    public async Task<ActionResult<AppointmentDto>> Put(string appointmentId, AppointmentDto appointmentDto)
    {
        try
        {
            if (!appointmentId.Equals(appointmentDto.Id)) ErrorService.IdError(appointmentId);
            var appointment = await _appointmentRepository.UpdateAppointmentAsync(appointmentDto);
            return Ok(appointment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not update appointment",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }

    [HttpDelete("{appointmentId}")]
    public async Task<ActionResult<bool>> Delete(string appointmentId)
    {
        try
        {
            var checkDelete = await _appointmentRepository.DeleteAppointmentByIdAsync(appointmentId);
            if (!checkDelete) throw new Exception();
            return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            var errorResponse = ErrorService.CreateError("Could not delete appointment",
                StatusCodes.Status400BadRequest, e.Message);
            return BadRequest(errorResponse);
        }
    }
}