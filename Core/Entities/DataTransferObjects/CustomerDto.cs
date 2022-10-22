using System.ComponentModel.DataAnnotations;

namespace Core.Entities.DataTransferObjects;

public class CustomerDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] [StringLength(200)] public string? FirstName { get; set; }

    [Required] [StringLength(200)] public string? LastName { get; set; }

    public AddressDto AddressDto { get; set; } = new();

    [Required] public string? AddressId { get; set; }

    [StringLength(200)] public string? Email { get; set; }

    [StringLength(100)] public string? PhoneNumber { get; set; }

    public string? UserId { get; set; }
}