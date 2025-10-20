using Cnab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cnab.Tests;

[Collection(nameof(DatabaseCollection))]
public abstract class DatabaseTest : IDisposable
{
    private readonly DatabaseFixture _fixture;
    private AppDbContext? _dbContext;
    private IDbContextTransaction? _transaction;

    protected DatabaseTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    protected AppDbContext DbContext
    {
        get
        {
            if (_dbContext == null)
            {
                _dbContext = _fixture.CreateDbContext();
                _transaction = DbContext.Database.BeginTransaction();
            }

            return _dbContext;
        }
    }

    protected virtual Action<DbContextOptionsBuilder<AppDbContext>>?
        DbContextOptions => null;

    protected virtual Action<SqlServerDbContextOptionsBuilder>?
        SqlServerDbContextOptions => null;

    public virtual void Dispose()
    {
        _transaction?.Rollback();
        _dbContext?.Dispose();
    }
}
