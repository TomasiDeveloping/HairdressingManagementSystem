using System.ComponentModel.DataAnnotations;

namespace Core.Entities.DataTransferObjects;

public class AppointmentDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public CustomerDto CustomerDto { get; set; } = new();

    [Required] public string? CustomerId { get; set; }

    public EmployeeDto EmployeeDto { get; set; } = new();

    [Required] public string? EmployeeId { get; set; }

    [Required] public DateTime AppointmentDate { get; set; }

    public string? Note { get; set; }
    public decimal? Price { get; set; }
}