using Core.Entities.DataTransferObjects;

namespace Core.Interfaces;

public interface IAddressRepository
{
    Task<AddressDto?> GetAddressByIdAsync(string addressId);
    Task<AddressDto> CreateAddressAsync(AddressDto addressDto);
    Task<AddressDto> UpdateAddressAsync(AddressDto addressDto);
    Task<bool> DeleteAddressByAddressIdAsync(string addressId);
}