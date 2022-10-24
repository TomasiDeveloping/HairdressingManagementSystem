using Core.Entities.DataTransferObjects;
using Core.Entities.Models;

namespace Core.Models;

public class CustomerForRegistration
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressDto Address { get; set; } = new();
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
}