using Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configuration;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Address)
            .WithMany()
            .HasForeignKey(e => e.AddressId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(e => e.Id).HasMaxLength(450).IsRequired();
        builder.Property(e => e.AddressId).HasMaxLength(450).IsRequired();
        builder.Property(e => e.UserId).HasMaxLength(450).IsRequired(false);

        builder.Property(e => e.FirstName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.PhoneNumber).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.BirthDate).IsRequired();
        builder.Property(e => e.JobTitle).HasMaxLength(150).IsRequired();
        builder.Property(e => e.WorkPhone).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.WorkEmail).HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired(false);
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
    }
}