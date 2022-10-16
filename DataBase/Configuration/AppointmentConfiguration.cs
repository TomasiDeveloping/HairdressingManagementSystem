using Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configuration;

internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Customer)
            .WithMany()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(a => a.Id).HasMaxLength(450).IsRequired();
        builder.Property(a => a.CustomerId).HasMaxLength(450).IsRequired();
        builder.Property(a => a.EmployeeId).HasMaxLength(450).IsRequired();

        builder.Property(a => a.AppointmentDate).IsRequired();
        builder.Property(a => a.Note).IsRequired(false);
        builder.Property(a => a.Price).HasPrecision(18, 2).IsRequired(false);
    }
}