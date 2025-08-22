using System.Collections.Concurrent;
using System.Security.Claims;
using Steria.Core.CQRS.Chat;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Steria.API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub(IMediator mediator) : Hub //TODO: Move userConnections to redis
{
    private static ConcurrentDictionary<int, string> _userConnections = new ConcurrentDictionary<int, string>();
    
    public async Task JoinChat(int chatId)
    {
        var userId = GetUserId(Context);
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId});
        if (!isUserInChat) throw new HubException("User is not a participant of this chat.");
        
        await Groups.AddToGroupAsync(Context.ConnectionId, "Chat"+chatId);

        var res = await mediator.Send(new GetChatMessagesQuery { ChatId = chatId, CurrentUserId = userId});
        await Clients.Caller.SendAsync("ReceiveChatHistory", res);
    }
    
    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Chat"+chatId);
    }

    public async Task SendMessage(int chatId, string message, List<IFormFile> images)
    {
        var senderId = GetUserId(Context);

        try
        {
            var newMessage = await mediator.Send(new SendChatMessageCommand
            {
                Attachments = images,
                ChatId = chatId,
                Message = message,
                SenderId = senderId
            });

            await Clients.Group("Chat" + chatId).SendAsync("ReceiveMessage", newMessage);
        }
        catch (ValidationException ex)
        {
            throw new HubException(ex.Message);
        }
    }

    public async Task EditMessage(int chatId, int messageId, string newMessage)
    {
        var userId = GetUserId(Context);

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
        var requesterId = GetUserId(Context);

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
        var userId = GetUserId(Context);
        
        var command = new DeleteAttachmentsCommand
        {
            ChatId = chatId,
            AttachmentIds = attachmentsId,
            UserId = userId
        };
        var deletedAttachmentIds = await mediator.Send(command);
        await Clients.Group("Chat" + chatId).SendAsync("AttachmentsDeleted", deletedAttachmentIds);
    }

    public async Task SendTypingStatus(int chatId, bool isTyping)
    {
        int userId = GetUserId(Context);

        await Clients.OthersInGroup("Chat" + chatId).SendAsync("ReceiveTypingStatus",
            new { ChatId = chatId, UserId = userId, isTyping = isTyping });
    }

    public async Task ReadMessage(int chatId, int messageId)
    {
        int userId = GetUserId(Context);

        var command = new CreateMessageReactionCommand { ChatId = chatId, MessageId = messageId, ReaderId = userId };
        var msgSenderId = await mediator.Send(command);

        await Clients.Clients(_userConnections.Where(userId => userId.Key == msgSenderId).Select(p => p.Value))
                     .SendAsync("MessageSeen", new { ChatId = chatId, MessageId = messageId, ReaderId = userId });
    }

    public async Task ToggleEmoji(string emoji, int messageId, int chatId)
    {
        int userId = GetUserId(Context);

        var command = new ToggleEmojiCommand { UserId = userId, Emoji = emoji, MessageId = messageId, ChatId = chatId };
        var isCreated = await mediator.Send(command);

        await Clients.Group("Chat" + chatId).SendAsync("ReactionChange", new { isCreated = isCreated, emoji = emoji });
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId(Context);

        var query = new GetConnectedUsersIdQuery { CurrentUserId = userId, TargetUserIds = _userConnections.Keys.ToList() };
        var onlineUserIds = await mediator.Send(query);

        await Clients.Caller.SendAsync("UsersOnline", onlineUserIds);

        await Clients.Clients(_userConnections.Where(userId => onlineUserIds.Contains(userId.Key))
                            .Select(u => u.Value))
            .SendAsync("NewOnlineUser", userId);

        _userConnections.TryAdd(userId, Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId(Context);

        var query = new GetConnectedUsersIdQuery { CurrentUserId = userId, TargetUserIds = _userConnections.Keys.ToList() };
        var onlineUserIds = await mediator.Send(query);

        await Clients.Clients(
                _userConnections.Where(u => onlineUserIds.Contains(u.Key))
                    .Select(u => u.Value))
            .SendAsync("NewOfflineUser", userId);

        _userConnections.Remove(userId, out var value);
        
        await base.OnDisconnectedAsync(exception);
    }

    private int GetUserId(HubCallerContext context)
    {
        return int.Parse(context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
    
}