namespace Core.Entities.Models;

public class Customer
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Address Address { get; set; } = new();
    public string? AddressId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public User? User { get; set; }
    public string? UserId { get; set; }
}