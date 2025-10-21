using Cnab.Application.Dto;
using MediatR;

namespace Cnab.Application.Queries.GetAccountTransactions;

public class GetAccountTransactionsQuery : IRequest<IEnumerable<AccountTransactionDto>>
{
    public int StoreId { get; set; }

    public GetAccountTransactionsQuery(int storeId)
    {
        StoreId = storeId;
    }
}
