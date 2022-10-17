using Core.Entities.DataTransferObjects;

namespace Core.Interfaces;

public interface ICustomerRepository
{
    Task<List<CustomerDto>> GetCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(string customerId);
    Task<CustomerDto> UpdateCustomerAsync(CustomerDto customerDto);
    Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto);
    Task<bool> DeleteCustomerByCustomerId(string customerId);
}