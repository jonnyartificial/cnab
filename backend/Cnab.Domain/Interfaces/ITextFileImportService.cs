using Cnab.Domain.ValueObjects;

namespace Cnab.Domain.Interfaces;

public interface ITextFileImportService
{
    Task<TextFileParseResult> ImportAsync(string content, CancellationToken cancellationToken);
}
