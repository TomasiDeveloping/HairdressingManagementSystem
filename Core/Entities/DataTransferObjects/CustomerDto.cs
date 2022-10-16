namespace Core.Entities.DataTransferObjects;

public class CustomerDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressDto AddressDto { get; set; } = new();
    public string? AddressId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserId { get; set; }
}