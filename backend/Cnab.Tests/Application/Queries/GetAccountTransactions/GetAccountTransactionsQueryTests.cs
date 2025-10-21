using Cnab.Application.Queries.GetAccountTransactions;
using Cnab.Domain.Entities;
using Cnab.Infrastructure.Repositories;

namespace Cnab.Tests.Application.Queries.GetAccountTransactions;

[Collection(nameof(DatabaseCollection))]
public class GetAccountTransactionsQueryTests : DatabaseTest
{
    public GetAccountTransactionsQueryTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetAccountTransaction_Returns_Record()
    {
        //arrange
        var store = new Store
        {
            Name = "BAR DO JOÃO"
        };
        await DbContext.AddAsync(store);
        await DbContext.SaveChangesAsync();

        var entity = new AccountTransaction
        {
            TransactionTypeId = 1,
            StoreId = store.Id,
            Date = new DateTime(2019, 03, 01, 23, 30, 00),
            Value = 152.00m,
            Cpf = "09620676017",
            Card = "1234****7890",
            StoreOwner = "JOÃO MACEDO",
            CreatedDateTime = new DateTime(2019, 03, 03, 12, 13, 14)
        };

        await DbContext.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var query = new GetAccountTransactionsQuery(store.Id);
        var handler = new GetAccountTransactionsHandler(new AccountTransactionRepository(DbContext));

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.Single(result);

        var record = result.First();
        Assert.Equal("Debit", record.Type);
        Assert.Equal(new DateTime(2019, 03, 01, 23, 30, 00), record.Date);
        Assert.Equal(152.00m, record.Value);
        Assert.Equal("09620676017", record.Cpf);
        Assert.Equal("1234****7890", record.Card);
        Assert.Equal("JOÃO MACEDO", record.StoreOwner);
        Assert.Equal("BAR DO JOÃO", record.StoreName);
    }

    [Fact]
    public async Task GetAccountTransaction_ForOneStore_Returns_Records()
    {
        //arrange
        var store = new Store
        {
            Name = "BAR DO JOÃO"
        };
        await DbContext.AddAsync(store);
        await DbContext.SaveChangesAsync();
        
        var entity1 = new AccountTransaction
        {
            TransactionTypeId = 1,
            StoreId = store.Id,
            Date = new DateTime(2019, 03, 01, 23, 30, 00),
            Value = 152.00m,
            Cpf = "09620676017",
            Card = "1234****7890",
            StoreOwner = "JOÃO MACEDO",
            CreatedDateTime = new DateTime(2019, 03, 03, 12, 13, 14)
        };

        var entity2 = new AccountTransaction
        {
            TransactionTypeId = 3,
            StoreId = store.Id,
            Date = new DateTime(2019, 03, 01, 15, 34, 53),
            Value = 142.00m,
            Cpf = "09620676017",
            Card = "4753****3153",
            StoreOwner = "JOÃO MACEDO",
            CreatedDateTime = new DateTime(2019, 03, 03, 12, 13, 14)
        };

        var entity3 = new AccountTransaction
        {
            TransactionTypeId = 2,
            StoreId = store.Id,
            Date = new DateTime(2019, 03, 01, 23, 42, 34),
            Value = 112.00m,
            Cpf = "09620676017",
            Card = "3648****0099",
            StoreOwner = "JOÃO MACEDO",
            CreatedDateTime = new DateTime(2019, 03, 03, 12, 13, 14)
        };

        await DbContext.AddRangeAsync(entity1, entity2, entity3);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var query = new GetAccountTransactionsQuery(store.Id);
        var handler = new GetAccountTransactionsHandler(new AccountTransactionRepository(DbContext));

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

        var record1 = result.First();
        Assert.Equal(new DateTime(2019, 03, 01, 15, 34, 53), record1.Date);
        Assert.Equal(-142.00m, record1.Value);
        Assert.Equal(-142.00m, record1.Balance);

        var record2 = result.ElementAt(1);
        Assert.Equal(new DateTime(2019, 03, 01, 23, 30, 00), record2.Date);
        Assert.Equal(152.00m, record2.Value);
        Assert.Equal(10.00m, record2.Balance);

        var record3 = result.Last();
        Assert.Equal(new DateTime(2019, 03, 01, 23, 42, 34), record3.Date);
        Assert.Equal(-112.00m, record3.Value);
        Assert.Equal(-102.00m, record3.Balance);

    }
}
