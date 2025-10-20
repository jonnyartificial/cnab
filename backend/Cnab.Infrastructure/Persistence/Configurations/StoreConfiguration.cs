using Cnab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cnab.Infrastructure.Persistence.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(19);
    }
}

