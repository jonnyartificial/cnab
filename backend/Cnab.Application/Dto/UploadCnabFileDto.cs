namespace Cnab.Application.Dto;

public class UploadCnabFileDto
{
    public bool IsSuccess { get; set; }
    public int LinesRead { get; set; }
    public int Errors { get; set; }
    public int Imported { get; set; }
    public IList<string> Messages { get; set; } = [];
}
