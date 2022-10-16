namespace Core.Entities.Models;

public class Address
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public int Zip { get; set; }
    public string? City { get; set; }
    public string? AddressAddition { get; set; }
}