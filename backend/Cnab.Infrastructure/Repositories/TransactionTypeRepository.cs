using Cnab.Domain.Entities;
using Cnab.Domain.Interfaces;
using Cnab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Infrastructure.Repositories;

public class TransactionTypeRepository : ITransactionTypeRepository
{
    private readonly AppDbContext _context;

    public TransactionTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransactionType>> GetAllTransactionTypesAsync(CancellationToken cancellationToken)
    {
        return await _context.TransactionTypes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
