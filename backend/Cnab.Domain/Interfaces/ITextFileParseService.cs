using Cnab.Domain.ValueObjects;

namespace Cnab.Domain.Interfaces;

public interface ITextFileParseService
{
    Task<TextFileParseResult> ParseAsync(string content, CancellationToken cancellationToken);
}
