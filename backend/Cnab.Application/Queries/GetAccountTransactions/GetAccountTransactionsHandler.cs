using Cnab.Application.Dto;
using Cnab.Domain.Interfaces;
using MediatR;

namespace Cnab.Application.Queries.GetAccountTransactions;

public class GetAccountTransactionsHandler : IRequestHandler<GetAccountTransactionsQuery, IEnumerable<AccountTransactionDto>>
{
    private readonly IAccountTransactionRepository _repository;

    public GetAccountTransactionsHandler(IAccountTransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AccountTransactionDto>> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var result = new List<AccountTransactionDto>();

        var records = await _repository.Get(request.StoreId, cancellationToken);

        decimal balance = 0;

        foreach (var record in records)
        {
            var value = record.TransactionType.IsIncome
                ? record.Value
                : record.Value * -1;

            balance += value;

            result.Add(new AccountTransactionDto
            {
                Id = record.Id,
                Type = record.TransactionType.Description,
                Date = record.Date,
                Value = value,
                Balance = balance,
                Cpf = record.Cpf,
                Card = record.Card,
                StoreOwner = record.StoreOwner,
                StoreName = record.Store.Name,
            });
        }

        return result;
    }
}
