using Cnab.Application.Commands.UploadCnabFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Cnab")]
    public async Task<IActionResult> UploadTextFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");

        if (Path.GetExtension(file.FileName).ToLower() != ".txt")
            return BadRequest("Not a .txt file");

        string content;
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            content = await reader.ReadToEndAsync();
        }

        var command = new UploadCnabFileCommand(file.FileName, content);

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(result);

        return StatusCode(500, "Failed to import file.");
    }
}
