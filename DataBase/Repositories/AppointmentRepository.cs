using AutoMapper;
using Core.Entities.DataTransferObjects;
using Core.Entities.Models;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;

    public AppointmentRepository(RepositoryContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(string appointmentId)
    {
        if (_context.Appointments == null) throw new ArgumentNullException(nameof(_context.Appointments));
        var appointment = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Employee)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id.Equals(appointmentId));
        return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByCustomerIdAsync(string customerId)
    {
        if (_context.Appointments == null) throw new ArgumentNullException(nameof(_context.Appointments));
        var appointments = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Employee)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(a => a.CustomerId != null && a.CustomerId.Equals(customerId))
            .ToListAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<List<AppointmentDto>> GetAppointmentsByEmployeeIdAsync(string employeeId)
    {
        if (_context.Appointments == null) throw new ArgumentNullException(nameof(_context.Appointments));
        var appointments = await _context.Appointments
            .Include(a => a.Customer)
            .Include(a => a.Employee)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(a => a.EmployeeId != null && a.EmployeeId.Equals(employeeId))
            .ToListAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentDto)
    {
        if (_context.Appointments == null) throw new ArgumentNullException(nameof(_context.Appointments));
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> UpdateAppointmentAsync(AppointmentDto appointmentDto)
    {
        if (_context.Appointments == null) throw new ArgumentNullException(nameof(_context.Appointments));
        var appointmentToUpdate =
            await _context.Appointments.FirstOrDefaultAsync(a => a.Id.Equals(appointmentDto.Id));
        if (appointmentToUpdate == null)
            throw new AggregateException($"No appointment found with id: {appointmentDto.Id}");
        _mapper.Map(appointmentDto, appointmentToUpdate);
        await _context.SaveChangesAsync();
        return _mapper.Map<AppointmentDto>(appointmentToUpdate);
    }

    public async Task<bool> DeleteAppointmentByIdAsync(string appointmentId)
    {
        if (_context.Appointments == null) throw new ArgumentNullException(nameof(_context.Appointments));
        var appointmentToDelete = await _context.Appointments.FirstOrDefaultAsync(a => a.Id.Equals(appointmentId));
        if (appointmentToDelete == null)
            throw new AggregateException($"No appointment found with id: {appointmentId}");
        _context.Appointments.Remove(appointmentToDelete);
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