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
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync()
    {
        if (_context.Employees == null) return new List<EmployeeDto>();
        var employees = await _context.Employees
            .Include(e => e.Address)
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<List<EmployeeDto>>(employees);
    }

    public Task<EmployeeDto> GetEmployeeByIdAsync(string employeeId)
    {
        throw new NotImplementedException();
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
    {
        var employee = _mapper.Map<Employee>(employeeDto);
        employee.Address = _mapper.Map<Address>(employeeDto.AddressDto);
        if (_context.Employees != null) await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return _mapper.Map<EmployeeDto>(employee);
    }

    public Task<EmployeeDto> UpdateEmployeeAsync(EmployeeDto employeeDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteEmployeeAsync(string employeeId)
    {
        throw new NotImplementedException();
    }
}