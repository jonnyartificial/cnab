using Cnab.Application.Dto;
using Cnab.Domain.Interfaces;
using MediatR;

namespace Cnab.Application.Queries.GetAllStores;

public class GetAllStoresHandler : IRequestHandler<GetAllStoresQuery, IEnumerable<StoreDto>>
{
    private readonly IStoreRepository _repository;

    public GetAllStoresHandler(IStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StoreDto>> Handle(
        GetAllStoresQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllStoresAsync(cancellationToken);

        return result.Select(p => new StoreDto
        {
            Id = p.Id,
            Name = p.Name,
        });
    }
}
