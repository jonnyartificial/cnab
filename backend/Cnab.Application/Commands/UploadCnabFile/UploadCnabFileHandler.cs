using Cnab.Application.Dto;
using Cnab.Domain.Interfaces;
using MediatR;

namespace Cnab.Application.Commands.UploadCnabFile;

public class UploadCnabFileHandler : IRequestHandler<UploadCnabFileCommand, UploadCnabFileDto>
{
    private readonly ITextFileImportService _textFileParserService;

    public UploadCnabFileHandler(ITextFileImportService textFileParserService)
    {
        _textFileParserService = textFileParserService;
    }

    public async Task<UploadCnabFileDto> Handle(UploadCnabFileCommand request, CancellationToken cancellationToken)
    {
        var content = new string(request.Content);

        var result = await _textFileParserService.ImportAsync(content, cancellationToken);

        return new UploadCnabFileDto
        {
            IsSuccess = result.IsSuccess,
            LinesRead = result.LinesRead,
            Errors = result.Erros,
            Imported = result.Imported,
            Messages = result.Messages,
        };
    }
}
