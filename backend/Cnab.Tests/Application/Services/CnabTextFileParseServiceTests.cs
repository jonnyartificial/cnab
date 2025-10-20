using Cnab.Application.Commands.UploadCnabFile;
using Cnab.Application.Services;
using Cnab.Infrastructure.Repositories;

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
            new TransactionTypeRepository(DbContext),
            new StoreRepository(DbContext)));

        //act
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.LinesRead);
        Assert.Equal(0, result.Errors);
        Assert.Equal(0, result.Imported);
        Assert.Empty(result.Messages);
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
            new TransactionTypeRepository(DbContext),
            new StoreRepository(DbContext)));

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
