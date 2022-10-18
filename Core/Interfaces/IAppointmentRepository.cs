using Core.Entities.DataTransferObjects;

namespace Core.Interfaces;

public interface IAppointmentRepository
{
    Task<AppointmentDto?> GetAppointmentByIdAsync(string appointmentId);
    Task<List<AppointmentDto>> GetAppointmentsByCustomerIdAsync(string customerId);
    Task<List<AppointmentDto>> GetAppointmentsByEmployeeIdAsync(string employeeId);
    Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto);
    Task<AppointmentDto> UpdateAppointmentAsync(AppointmentDto appointmentDto);
    Task<bool> DeleteAppointmentByIdAsync(string appointmentId);
}