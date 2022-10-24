using Core.Entities.DataTransferObjects;

namespace Core.Models;

public class EmployeeForRegistration
{
    public AddressDto AddressDto { get; set; } = new();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string? JobTitle { get; set; }
    public string? WorkPhone { get; set; }
    public string? WorkEmail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
    public string? Password { get; set; }
}