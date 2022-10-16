namespace Core.Entities.Models;

public class Appointment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Customer Customer { get; set; } = new();
    public string? CustomerId { get; set; }
    public Employee Employee { get; set; } = new();
    public string? EmployeeId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Note { get; set; }
    public decimal? Price { get; set; }
}