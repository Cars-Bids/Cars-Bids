using Steria.Core.CQRS.Chat;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Steria.API.Hubs;

namespace Steria.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatController(IMediator mediator, IHubContext<ChatHub> chatHubContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatCommand command)
    {
        var chatId = await mediator.Send(command);
        
        foreach (var userId in command.ParticipantIds)
        {
            await chatHubContext.Clients.User(userId.ToString()).SendAsync("ChatCreated", chatId);
        }
        
        return Ok(chatId);
    }
}