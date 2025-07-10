using System.Collections.Concurrent;
using System.Security.Claims;
using CarsAndBids.Core.CQRS.Chat;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CarsAndBids.API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub(IMediator mediator) : Hub
{
    private static readonly ConcurrentDictionary<int, bool> UserStatuses = new ConcurrentDictionary<int, bool>();
    
    public async Task JoinChat(int chatId)
    {
        var userId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId});
        if (!isUserInChat)
        {
            throw new HubException("User is not a participant of this chat.");
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, "Chat"+chatId);

        var res = mediator.Send(new GetChatMessagesQuery { ChatId = chatId });
        await Clients.Caller.SendAsync("ReceiveChatHistory", res.Result);
    }
    
    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Chat"+chatId);
    }

    public async Task SendMessage(int chatId, string message, List<IFormFile> images)
    {
        var senderId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var newMessage = await mediator.Send(new SendChatMessageCommand
        {
            Attachments = images,
            ChatId = chatId,
            Message = message,
            SenderId = senderId
        });


        await Clients.Group("Chat"+chatId).SendAsync("ReceiveMessage", newMessage);
    }

    public async Task EditMessage(int chatId, int messageId, string newMessage)
    {
        var userId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var editedMessage = await mediator.Send(new EditChatMessageCommand
        {
            UserId = userId,
            ChatId = chatId, 
            MessageId = messageId,
            NewMessage = newMessage
        });
        
        await Clients.Group("Chat"+chatId).SendAsync("EditMessage", editedMessage);
    }
    
    public async Task DeleteMessage(int messageId, int chatId)
    {
        var requesterId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        await mediator.Send(new DeleteMessageCommand
        {
            MessageId = messageId,
            ChatId = chatId,
            RequesterId = requesterId
        });

        await Clients.Group("Chat"+chatId).SendAsync("DeleteMessage", messageId);
    }

    public async Task DeleteAttachments(List<int> attachmentsId, int chatId)
    {
        var userId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var command = new DeleteAttachmentsCommand
        {
            ChatId = chatId,
            AttachmentIds = attachmentsId,
            UserId = userId
        };
        var deletedAttachmentIds = await mediator.Send(command);
        await Clients.Group("Chat" + chatId).SendAsync("AttachmentsDeleted", deletedAttachmentIds);
    }
    
    public override async Task OnConnectedAsync()
    {
        var nameId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine("UserId: " + nameId); // має бути userId
        await base.OnConnectedAsync();
    }
    
}