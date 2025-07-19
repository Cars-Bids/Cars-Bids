using Microsoft.AspNetCore.Mvc;
using MediatR;
using CarsAndBids.Core.CQRS.Emails;
using CarsAndBids.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarsAndBids.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController(IMediator mediator, UserManager<User> userManager) : ControllerBase
{
    [HttpPost("send-reset")]
    public async Task<IActionResult> SendPasswordResetEmail([FromBody] SendPasswordResetEmailCommand request)
    {
        await mediator.Send(request);
        return Ok();
    }
}