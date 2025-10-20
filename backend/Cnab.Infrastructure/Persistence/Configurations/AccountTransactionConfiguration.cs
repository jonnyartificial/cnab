using Cnab.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cnab.Infrastructure.Persistence.Configurations;

public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasOne(p => p.TransactionType)
            .WithMany(p => p.AccountTransactions)
            .HasForeignKey(p => p.TransactionTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Store)
            .WithMany(p => p.AccountTransactions)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(p => p.Date)
            .HasColumnType("datetime2(0)");

        builder
            .Property(p => p.Value)
            .HasColumnType("decimal(18,2)");

        builder
            .Property(p => p.Cpf)
            .HasMaxLength(11);

        builder
            .Property(p => p.Card)
            .HasMaxLength(12);

        builder
            .Property(p => p.StoreOwner)
            .HasMaxLength(14);

        builder
            .Property(p => p.CreatedDateTime)
            .HasColumnType("datetime2");

        //implement unique constraint to guarantee idempotence
        builder
            .HasIndex(p => new { p.TransactionTypeId, p.StoreId, p.Date, p.Value, p.Cpf, p.Card })
            .IsUnique();
    }
}
