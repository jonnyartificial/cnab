using Cnab.Application.Commands.UploadCnabFile;
using Cnab.Application.Services;
using Cnab.Domain.Entities;
using Cnab.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Tests.Application.Services;

[Collection(nameof(DatabaseCollection))]
public class CnabTextFileImportServiceTests : DatabaseTest
{
    public CnabTextFileImportServiceTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task ImportFileLine_Returns_Success()
    {
        //arrange
        var fileContent = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n";
        var command = new UploadCnabFileCommand("cnab.txt", fileContent);

        //act
        var result = await GetHandler().Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(0, result.Errors);
        Assert.Equal(1, result.Imported);
        Assert.Empty(result.Messages);

        var accountTransactions = await DbContext.AccountTransactions
            .Include(p => p.TransactionType)
            .Include(p => p.Store)
            .ToListAsync();

        Assert.NotNull(accountTransactions);
        Assert.Single(accountTransactions);

        var record = accountTransactions.First();
        Assert.NotNull(record.TransactionType);
        Assert.Equal(3, record.TransactionType.Id);
        Assert.Equal("BAR DO JOÃO", record.Store.Name);
        Assert.Equal(new DateTime(2019, 03, 01, 15, 34, 53), record.Date);
        Assert.Equal(142.00m, record.Value);
        Assert.Equal("09620676017", record.Cpf);
        Assert.Equal("4753****3153", record.Card);
        Assert.Equal("JOÃO MACEDO", record.StoreOwner);
    }

    [Theory]
    [InlineData(
        "A201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO\r\n",
        "Line 1: Invalid Length")]
    [InlineData(
        "A201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n",
        "Line 1: Invalid Type field")]
    [InlineData(
        "3201903320000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n",
        "Line 1: Invalid Date/Time fields")]
    [InlineData(
        "3201903010000A14200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n",
        "Line 1: Invalid Value field")]
    [InlineData(
        "320190301000001420009620A760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n",
        "Line 1: Invalid CPF field")]
    public async Task ImportFileLine_Returns_Success_with_Error(string fileContent, string errorMessage)
    {
        //arrange
        var command = new UploadCnabFileCommand("cnab.txt", fileContent);

        //act
        var result = await GetHandler().Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(1, result.Errors);
        Assert.Equal(0, result.Imported);
        Assert.Single(result.Messages);
        Assert.Equal(errorMessage, result.Messages.First());
    }

    [Fact]
    public async Task ImportFileLine_Fails_Idepotence()
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
            TransactionTypeId = 3,
            StoreId = store.Id,
            Date = new DateTime(2019, 03, 01, 15, 34, 53),
            Value = 142.00m,
            Cpf = "09620676017",
            Card = "4753****3153",
            StoreOwner = "JOÃO MACEDO",
            CreatedDateTime = new DateTime(2019, 03, 03, 12, 13, 14)
        };
        await DbContext.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var fileContent = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n";
        var command = new UploadCnabFileCommand("cnab.txt", fileContent);

        //act
        var result = await GetHandler().Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(1, result.Errors);
        Assert.Equal(0, result.Imported);
        Assert.Single(result.Messages);
        Assert.Equal("Line 1: This record was already imported", result.Messages.First());
    }

    private UploadCnabFileHandler GetHandler()
    {
         return new UploadCnabFileHandler(
            new CnabTextFileImportService(
            new EfUnitOfWork(DbContext),
            new TransactionTypeRepository(DbContext),
            new StoreRepository(DbContext),
            new AccountTransactionRepository(DbContext)));
    }
}
