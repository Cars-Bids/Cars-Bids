using CarsAndBids.Core.CQRS.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterCommand cmd)
    {
        try
        {
            await mediator.Send(cmd);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> LoginViaRefreshToken([FromBody] LoginViaRefreshTokenQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}