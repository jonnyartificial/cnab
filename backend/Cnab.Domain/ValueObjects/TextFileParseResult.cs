namespace Cnab.Domain.ValueObjects;

public class TextFileParseResult
{
    public bool IsSuccess { get; set; }
    public int LinesRead { get; set; }
    public int Erros { get; set; }
    public int Imported { get; set; }
    public IList<string> Messages { get; set; } = [];
}
