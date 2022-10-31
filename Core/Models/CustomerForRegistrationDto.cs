using Core.Entities.DataTransferObjects;

namespace Core.Models;

public class CustomerForRegistrationDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressDto Address { get; set; } = new();
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
}