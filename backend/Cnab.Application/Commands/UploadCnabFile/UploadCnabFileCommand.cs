using Cnab.Application.Dto;
using MediatR;

namespace Cnab.Application.Commands.UploadCnabFile;

public class UploadCnabFileCommand : IRequest<UploadCnabFileDto>
{
    public string FileName { get; }
    public string Content { get; }

    public UploadCnabFileCommand(string fileName, string content)
    {
        FileName = fileName;
        Content = content;
    }
}