using Cnab.Application.Queries.GetAccountTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountTransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccountTransactions(int storeId)
    {
        var result = await _mediator.Send(new GetAccountTransactionsQuery(storeId));
        return Ok(result);
    }
}
