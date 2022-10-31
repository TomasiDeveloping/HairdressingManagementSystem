using System.ComponentModel.DataAnnotations;

namespace Core.Entities.DataTransferObjects;

public class EmployeeDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public AddressDto AddressDto { get; set; } = new();

    [Required] public string? AddressId { get; set; }

    [Required] [StringLength(200)] public string? FirstName { get; set; }

    [Required] [StringLength(200)] public string? LastName { get; set; }

    [StringLength(200)] public string? Email { get; set; }

    [StringLength(100)] public string? PhoneNumber { get; set; }

    [Required] public DateTime BirthDate { get; set; }

    [Required] [StringLength(150)] public string? JobTitle { get; set; }

    [StringLength(100)] public string? WorkPhone { get; set; }

    [StringLength(200)] public string? WorkEmail { get; set; }

    [Required] public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required] public bool IsActive { get; set; }

    public string? UserId { get; set; }
}