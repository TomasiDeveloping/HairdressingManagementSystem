using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;

    public EmployeeRepository(RepositoryContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync()
    {
        if (_context.Employees == null) throw new ArgumentNullException(nameof(_context));
        var employees = await _context.Employees
            .Include(e => e.Address)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();
        return _mapper.Map<List<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(string employeeId)
    {
        if (_context.Employees == null) throw new ArgumentNullException(nameof(_context));
        var employee = await _context.Employees
            .Include(e => e.Address)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id.Equals(employeeId));
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
    {
        if (_context.Employees == null) throw new ArgumentNullException(nameof(_context));
        var employee = _mapper.Map<Employee>(employeeDto);
        employee.Address = _mapper.Map<Address>(employeeDto.AddressDto);
        await _context.SaveChangesAsync();
        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(EmployeeDto employeeDto)
    {
        if (_context.Employees == null) throw new ArgumentNullException(nameof(_context));
        var employeeToUpdate = await _context.Employees.FirstOrDefaultAsync(e => e.Id.Equals(employeeDto.Id));
        if (employeeToUpdate == null) throw new ArgumentException($"No employee found with id: {employeeDto.Id}");
        _mapper.Map(employeeDto, employeeToUpdate);
        await _context.SaveChangesAsync();
        return _mapper.Map<EmployeeDto>(employeeToUpdate);
    }

    public async Task<bool> DeleteEmployeeAsync(string employeeId)
    {
        if (_context.Employees == null) throw new ArgumentNullException(nameof(_context));
        var employeeToDelete = await _context.Employees.FirstOrDefaultAsync(e => e.Id.Equals(employeeId));
        if (employeeToDelete == null) throw new AggregateException($"No employee found with id: {employeeId}");
        _context.Employees.Remove(employeeToDelete);
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