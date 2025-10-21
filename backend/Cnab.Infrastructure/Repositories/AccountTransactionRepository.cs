using Cnab.Domain.Entities;
using Cnab.Domain.Interfaces;
using Cnab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Infrastructure.Repositories;

public class AccountTransactionRepository : IAccountTransactionRepository
{
    private readonly AppDbContext _context;

    public AccountTransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AccountTransaction accountTransaction, CancellationToken cancellationToken)
    {
        await _context.AccountTransactions.AddAsync(accountTransaction, cancellationToken);
    }

    public async Task<IEnumerable<AccountTransaction>> Get(int storeId, CancellationToken cancellationToken)
    {
        return await _context.AccountTransactions
            .Where(p => p.StoreId == storeId)
            .Include(p => p.TransactionType)
            .Include(p => p.Store)
            .OrderBy(p => p.Date)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

