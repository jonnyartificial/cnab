using Cnab.Domain.Entities;
using Cnab.Domain.Interfaces;
using Cnab.Infrastructure.Persistence;

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
}

