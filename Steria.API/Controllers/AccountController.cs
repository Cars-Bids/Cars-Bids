using Steria.Core.CQRS.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCommand cmd) //TODO: need to add userNotificationSettings when creating account
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

    [HttpPost]
    public async Task<IActionResult> SendPasswordResetEmail([FromBody] SendPasswordResetEmailCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }
}