using Cnab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cnab.Infrastructure.Persistence.Configurations;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Description)
            .HasMaxLength(20);
    }
}
