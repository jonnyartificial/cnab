using Cnab.Domain.Entities;

namespace Cnab.Domain.Interfaces;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetAllStoresAsync(CancellationToken cancellationToken);

    Task<Store> Add(string name, CancellationToken cancellationToken);
}
