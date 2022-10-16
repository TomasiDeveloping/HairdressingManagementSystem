namespace Core.Entities.DataTransferObjects;

public class EmployeeDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public AddressDto AddressDto { get; set; } = new();
    public string? AddressId { get; set; }
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
    public bool IsActive { get; set; }
    public string? UserId { get; set; }
}