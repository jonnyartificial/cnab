using Cnab.Domain.Entities;
using Cnab.Domain.Interfaces;
using Cnab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Infrastructure.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Store>> GetAllStoresAsync(CancellationToken cancellationToken)
    {
        return await _context.Stores
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
