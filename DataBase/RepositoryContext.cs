using Core.Entities.Models;
using DataBase.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class RepositoryContext : IdentityDbContext<User>
{
    public RepositoryContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new AddressConfiguration());
        builder.ApplyConfiguration(new EmployeeConfiguration());
        builder.ApplyConfiguration(new CustomerConfiguration());
        builder.ApplyConfiguration(new AppointmentConfiguration());
    }

    public DbSet<Address>? Addresses { get; set; }
    public DbSet<Customer>? Customers { get; set; }
    public DbSet<Employee>? Employees { get; set; }
    public DbSet<Appointment>? Appointments { get; set; }
}