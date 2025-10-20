using Cnab.Application.Commands.UploadCnabFile;
using Cnab.Application.Services;
using Cnab.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Tests.Application.Services;

public class CnabTextFileParseServiceTests : DatabaseTest
{
    public CnabTextFileParseServiceTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task ParseFileLine_Returns_Success()
    {
        //arrange
        var fileContent = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n";
        var command = new UploadCnabFileCommand("cnab.txt", fileContent);
        var handler = new UploadCnabFileHandler(new CnabTextFileParseService(
            new EfUnitOfWork(DbContext),
            new TransactionTypeRepository(DbContext),
            new StoreRepository(DbContext),
            new AccountTransactionRepository(DbContext)));

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(0, result.Errors);
        Assert.Equal(0, result.Imported);
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
    public async Task ParseFileLine_Returns_Success_with_Error(string fileContent, string errorMessage)
    {
        //arrange
        var command = new UploadCnabFileCommand("cnab.txt", fileContent);
        var handler = new UploadCnabFileHandler(new CnabTextFileParseService(
            new EfUnitOfWork(DbContext),
            new TransactionTypeRepository(DbContext),
            new StoreRepository(DbContext),
            new AccountTransactionRepository(DbContext)));

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(1, result.Errors);
        Assert.Equal(0, result.Imported);
        Assert.Single(result.Messages);
        Assert.Equal(errorMessage, result.Messages.First());
    }
}
