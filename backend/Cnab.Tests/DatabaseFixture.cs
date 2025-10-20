using Cnab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Tests;

public class DatabaseFixture : IDisposable
{
    public static readonly string ConnectionString =
        $"Server=(localdb)\\mssqllocaldb;Database={typeof(AppDbContext).FullName};Trusted_Connection=True;MultipleActiveResultSets=True;";

    public DatabaseFixture()
    {
        var dbContext = CreateDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        dbContext.Dispose();
    }

    public AppDbContext CreateDbContext(
        Action<DbContextOptionsBuilder<AppDbContext>>? optionsAction = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(ConnectionString);
        optionsAction?.Invoke(optionsBuilder);
        return new AppDbContext(optionsBuilder.Options);
    }

    public void Dispose()
    {
    }

}

