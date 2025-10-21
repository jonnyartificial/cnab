using Cnab.Application.Commands.UploadCnabFile;
using Cnab.Application.Services;
using Cnab.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cnab.Tests.Application.Commands.UploadCnabFile;

[Collection(nameof(DatabaseCollection))]
public class UploadCnabFileCommandTests : DatabaseTest
{
    public UploadCnabFileCommandTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task ImportFileLine_Returns_Success()
    {
        //arrange
        var fileContent = "3201903010000014200096206760174753****3153153453JOÃO MACEDO   BAR DO JOÃO       \r\n";
        var textFileImportedService = new CnabTextFileImportService(
            new EfUnitOfWork(DbContext),
            new TransactionTypeRepository(DbContext),
            new StoreRepository(DbContext),
            new AccountTransactionRepository(DbContext));

        var command = new UploadCnabFileCommand("cnab.txt", fileContent);
        var handler = new UploadCnabFileHandler(textFileImportedService);

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(0, result.Errors);
        Assert.Equal(1, result.Imported);
        Assert.Empty(result.Messages);
    }
}
