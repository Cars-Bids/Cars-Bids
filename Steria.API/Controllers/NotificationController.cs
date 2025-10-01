using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steria.Core.CQRS.Notification;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);

        await mediator.Send(new MarkNotificationReadCommand
        {
            UserId = userId,
            NotificationId = id
        });

        return Ok();
    }
}