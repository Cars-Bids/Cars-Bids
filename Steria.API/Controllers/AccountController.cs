using Steria.Core.CQRS.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.CQRS.Profile;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterCommand cmd)
    {
        await mediator.Send(cmd);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var tokens = await mediator.Send(query);
        return Ok(tokens);
    }

    [HttpPost]
    public async Task<IActionResult> LoginViaRefreshToken([FromBody] LoginViaRefreshTokenQuery query)
    {
        var tokens = await mediator.Send(query);
        return Ok(tokens);
    }

    [HttpPost]
    public async Task<IActionResult> SendPasswordResetEmail([FromBody] SendPasswordResetEmailCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
}