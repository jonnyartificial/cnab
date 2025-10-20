using Cnab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<AccountTransaction> AccountTransactions { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<Store> Stores { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
