using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;

    public CustomerRepository(RepositoryContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        if (_context.Customers == null) throw new ArgumentNullException(nameof(_context));
        var customers = await _context.Customers
            .Include(c => c.Address)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
        return _mapper.Map<List<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(string customerId)
    {
        if (_context.Customers == null) throw new ArgumentNullException(nameof(_context));
        var customer = await _context.Customers
            .Include(c => c.Address)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id.Equals(customerId));
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(CustomerDto customerDto)
    {
        if (_context.Customers == null) throw new ArgumentNullException(nameof(_context));
        var customerToUpdate = await _context.Customers.FirstOrDefaultAsync(c => c.Id.Equals(customerDto.Id));
        if (customerToUpdate == null) throw new AggregateException($"No customer found with id: {customerDto.Id}");
        _mapper.Map(customerDto, customerToUpdate);
        await _context.SaveChangesAsync();
        return _mapper.Map<CustomerDto>(customerToUpdate);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
    {
        if (_context.Customers == null) throw new ArgumentNullException(nameof(_context));
        var customer = _mapper.Map<Customer>(customerDto);
        customer.Address = _mapper.Map<Address>(customerDto.AddressDto);
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> DeleteCustomerByCustomerId(string customerId)
    {
        if (_context.Customers == null) throw new ArgumentNullException(nameof(_context));
        var customerToDelete = await _context.Customers.FirstOrDefaultAsync(c => c.Id.Equals(customerId));
        if (customerToDelete == null) throw new AggregateException($"No customer found with id: {customerId}");
        _context.Customers.Remove(customerToDelete);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw new AggregateException(e.Message);
        }
    }
}