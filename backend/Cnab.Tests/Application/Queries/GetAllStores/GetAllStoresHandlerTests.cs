using Cnab.Application.Queries.GetAllStores;
using Cnab.Domain.Entities;
using Cnab.Infrastructure.Repositories;

namespace Cnab.Tests.Application.Queries.GetAllStores;

[Collection(nameof(DatabaseCollection))]
public class GetAllStoresHandlerTests : DatabaseTest
{
    public GetAllStoresHandlerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetAllStores_Returns_AllStores()
    {
        //arrange
        var entity = new Store
        {
            Name = "Store 1"
        };

        await DbContext.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var query = new GetAllStoresQuery();
        var handler = new GetAllStoresHandler(new StoreRepository(DbContext));

        //act
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(entity.Id, result.First().Id);
        Assert.Equal(entity.Name, result.First().Name);
    }
}
