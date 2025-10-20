using Cnab.Application.Dto;
using MediatR;

namespace Cnab.Application.Queries.GetAllStores;

public record GetAllStoresQuery : IRequest<IEnumerable<StoreDto>>;
