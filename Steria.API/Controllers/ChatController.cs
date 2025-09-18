using System.Security.Claims;
using Steria.Core.CQRS.Chat;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Steria.API.Hubs;
using Steria.Core.Constants;
using Steria.Core.DTOs;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    
    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAttachments([FromForm] SaveChatPhotosCommand command)
    {
        var urls = await mediator.Send(command);
        return Ok(urls);
    }
    
    [HttpGet("{chatId}/messages")]
    [Authorize]
    public async Task<IActionResult> GetMessages(int chatId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId });
        if (!isUserInChat)
        {
            return Forbid("User is not a participant of this chat.");
        }
        
        var query = new GetChatMessagesQuery { ChatId = chatId, CurrentUserId = userId, Page = page, PageSize = pageSize };
        var messages = await mediator.Send(query);
        
        var totalCountQuery = new ChatMessagesCountQuery { ChatId = chatId };
        var totalCount = await mediator.Send(totalCountQuery);

        return Ok(new { messages, totalCount });
    }

    [Authorize]
    [HttpGet("{chatId}/requirements")]
    public async Task<IActionResult> GetRequirements(int chatId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId });
        if (!isUserInChat)
        {
            return Forbid("User is not a participant of this chat.");
        }

        var query = new ChatRequirementsQuery { ChatId = chatId };
        var requirements = await mediator.Send(query);

        return Ok(requirements);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpPost("{chatId}/requirements")]
    public async Task<IActionResult> CreateRequirement(int chatId, [FromBody] CreateRequirementDto Dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId });
        if (!isUserInChat)
        {
            return Forbid("User is not a participant of this chat.");
        }

        var command = new CreateChatRequirementCommand { ChatId = chatId, ManagerId = userId, Text = Dto.Text };
        var dto = await mediator.Send(command);

        await chatHubContext.Clients.Group("Chat" + chatId).SendAsync("NewRequest", dto);
        
        return Ok(dto);
    }

    [Authorize(Roles = Roles.Manager)]
    [HttpDelete("{chatId}/requirements/{id}")]
    public async Task<IActionResult> DeleteRequirement(int chatId, int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId });
        if (!isUserInChat)
        {
            return Forbid("User is not a participant of this chat.");
        }

        await mediator.Send(new DeleteChatRequirementCommand { Id = id });
        await chatHubContext.Clients.Group("Chat" + chatId).SendAsync("RequestDeleted", id);
        
        return Ok();
    }

}