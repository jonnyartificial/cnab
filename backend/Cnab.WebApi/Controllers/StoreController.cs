using Cnab.Application.Queries.GetAllStores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StoreController : ControllerBase
{
    private readonly IMediator _mediator;

    public StoreController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetStores()
    {
        var result = await _mediator.Send(new GetAllStoresQuery());
        return Ok(result);
    }
}
