using Core.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataBase.Configuration;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasMaxLength(450).IsRequired();
        builder.Property(a => a.Street).HasMaxLength(250).IsRequired();
        builder.Property(a => a.HouseNumber).HasMaxLength(10).IsRequired();
        builder.Property(a => a.Zip).IsRequired();
        builder.Property(a => a.City).HasMaxLength(150).IsRequired();
        builder.Property(a => a.AddressAddition).HasMaxLength(250).IsRequired(false);
    }
}