using Core.Entities.DataTransferObjects;

namespace Core.Interfaces;

public interface IEmployeeRepository
{
    Task<List<EmployeeDto>> GetEmployeesAsync();
    Task<EmployeeDto> GetEmployeeByIdAsync(string employeeId);
    Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto);
    Task<EmployeeDto> UpdateEmployeeAsync(EmployeeDto employeeDto);
    Task<bool> DeleteEmployeeAsync(string employeeId);
}