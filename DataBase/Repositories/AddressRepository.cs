using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;

    public AddressRepository(RepositoryContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AddressDto?> GetAddressByIdAsync(string addressId)
    {
        if (_context.Addresses == null) throw new ArgumentNullException(nameof(_context));
        var address = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id.Equals(addressId));
        return address == null ? null : _mapper.Map<AddressDto>(address);
    }

    public async Task<AddressDto> CreateAddressAsync(AddressDto addressDto)
    {
        if (_context.Addresses == null) throw new ArgumentNullException(nameof(_context));
        var address = _mapper.Map<Address>(addressDto);
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
        return _mapper.Map<AddressDto>(address);
    }

    public async Task<AddressDto> UpdateAddressAsync(AddressDto addressDto)
    {
        if (_context.Addresses == null) throw new ArgumentNullException(nameof(_context));
        var addressToUpdate = await _context.Addresses.FirstOrDefaultAsync(a => a.Id.Equals(addressDto.Id));
        if (addressToUpdate == null) throw new ArgumentException($"No Address found with Id: {addressDto.Id}");
        _mapper.Map(addressDto, addressToUpdate);
        await _context.SaveChangesAsync();
        return _mapper.Map<AddressDto>(addressToUpdate);
    }

    public async Task<bool> DeleteAddressByAddressIdAsync(string addressId)
    {
        if (_context.Addresses == null) throw new ArgumentNullException(nameof(_context));
        var addressToDelete = await _context.Addresses.FirstOrDefaultAsync(a => a.Id.Equals(addressId));
        if (addressToDelete == null) throw new AggregateException($"No Address found with Id: {addressId}");
        _context.Addresses.Remove(addressToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}