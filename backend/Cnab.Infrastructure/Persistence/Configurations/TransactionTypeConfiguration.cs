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

        //load transaction type data
        //table id matches definition Id
        builder
            .HasData(
                new TransactionType { Id = 1, Description = "Debit", IsIncome = true },
                new TransactionType { Id = 2, Description = "Boleto", IsIncome = false },
                new TransactionType { Id = 3, Description = "Financing", IsIncome = false },
                new TransactionType { Id = 4, Description = "Credit", IsIncome = true },
                new TransactionType { Id = 5, Description = "Loan Receipt", IsIncome = true },
                new TransactionType { Id = 6, Description = "Sales", IsIncome = true },
                new TransactionType { Id = 7, Description = "TED Receipt", IsIncome = true },
                new TransactionType { Id = 8, Description = "DOC Receipt", IsIncome = true },
                new TransactionType { Id = 9, Description = "Rent", IsIncome = false }
            );
    }
}
