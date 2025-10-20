using Cnab.Domain.Entities;

namespace Cnab.Domain.Interfaces;

public interface ITransactionTypeRepository
{
    Task<IEnumerable<TransactionType>> GetAllTransactionTypesAsync(CancellationToken cancellationToken);
}
