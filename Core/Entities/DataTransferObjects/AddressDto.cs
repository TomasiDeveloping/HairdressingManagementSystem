using System.ComponentModel.DataAnnotations;

namespace Core.Entities.DataTransferObjects;

public class AddressDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required] [StringLength(250)] public string? Street { get; set; }

    [Required] [StringLength(10)] public string? HouseNumber { get; set; }

    [Required] public int Zip { get; set; }

    [Required] [StringLength(150)] public string? City { get; set; }

    [StringLength(250)] public string? AddressAddition { get; set; }
}