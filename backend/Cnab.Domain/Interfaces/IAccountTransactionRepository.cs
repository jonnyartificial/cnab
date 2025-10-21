using Cnab.Domain.Entities;

namespace Cnab.Domain.Interfaces;

public interface IAccountTransactionRepository
{
    Task AddAsync(AccountTransaction accountTransaction, CancellationToken cancellationToken);

    Task<IEnumerable<AccountTransaction>> Get(int storeId, CancellationToken cancellationToken);
}
