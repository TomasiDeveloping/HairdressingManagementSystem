using Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configuration;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasOne(c => c.Address)
            .WithMany()
            .HasForeignKey(c => c.AddressId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(c => c.Id).HasMaxLength(450).IsRequired();
        builder.Property(c => c.AddressId).HasMaxLength(450).IsRequired();
        builder.Property(c => c.UserId).HasMaxLength(450).IsRequired(false);

        builder.Property(c => c.FirstName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.PhoneNumber).HasMaxLength(100).IsRequired(false);
        builder.Property(c => c.Email).HasMaxLength(200).IsRequired(false);
    }
}