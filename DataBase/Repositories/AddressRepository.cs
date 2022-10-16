using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;

    public AddressRepository(RepositoryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AddressDto?> GetAddressByIdAsync(string addressId)
    {
        if (_context.Addresses == null) return null;
        var address = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id.Equals(addressId));
        return address == null ? null : _mapper.Map<AddressDto>(address);
    }

    public Task<AddressDto> CreateAddressAsync(AddressDto addressDto)
    {
        throw new NotImplementedException();
    }
}