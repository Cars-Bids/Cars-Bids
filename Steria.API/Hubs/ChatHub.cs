using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using Steria.Core.CQRS.Chat;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.CQRS.Notification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Notifications_Custom_Data;

namespace Steria.API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub(IMediator mediator) : Hub //TODO: add connectionManager instead of _userConnections
{
    private static ConcurrentDictionary<int, string> _userConnections = new ConcurrentDictionary<int, string>();
    
    public async Task JoinChat(int chatId)
    {
        var userId = GetUserId(Context);
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = chatId, UserId = userId});
        if (!isUserInChat) 
        {
            await Clients.Caller.SendAsync("JoinRejected", "User is not a participant of this chat.");
            return;
        }
        
        await Groups.AddToGroupAsync(Context.ConnectionId, "Chat" + chatId);
    }
    
    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Chat"+chatId);
    }

    public async Task SendMessage(int chatId, string message, List<string>? attachmentUrls = null)
    {
        var senderId = GetUserId(Context);

        try
        {
            var newMessage = await mediator.Send(new SendChatMessageCommand
            {
                AttachmentUrls = attachmentUrls,
                ChatId = chatId,
                Message = message,
                SenderId = senderId
            });

            await Clients.Group("Chat" + chatId).SendAsync("ReceiveMessage", newMessage);
            
            /*var chatUserIds = await mediator.Send(new GetChatUserIdsQuery { ChatId = chatId });
            var otherUserIds = chatUserIds.Where(id => id != senderId).ToList();

            var chat = await mediator.Send(new ChatInfoQuery { ChatId = chatId, CurentUser = senderId });
            
            var onlineOtherUsers = _userConnections.Keys.Intersect(otherUserIds).ToList();

            if (onlineOtherUsers.Count > 0)
            {
                await Clients.Group("Chat" + chatId).SendAsync("ReceiveMessage", newMessage);
            }
            else
            {
                foreach (var userId in otherUserIds)
                {
                    var customData = new ChatData
                    { 
                        ChatId = chatId, 
                        AuctionTitle = $"{chat.Make} {chat.Model}"
                    };
                    
                    await mediator.Send(new CreateNotificationCommand { NotificationTypeKey = "ChatMessage", CustomData = customData, UserId = userId});
                }
            }*/
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("MessageRejected", "Unexpected error: " + ex.Message);
        }
    }

    public async Task EditMessage(int chatId, int messageId, string newMessage)
    {
        var userId = GetUserId(Context);

        try
        {
            var editedMessage = await mediator.Send(new EditChatMessageCommand
            {
                UserId = userId,
                ChatId = chatId, 
                MessageId = messageId,
                NewMessage = newMessage
            });
            
            await Clients.Group("Chat"+chatId).SendAsync("EditMessage", editedMessage);
        }
        catch (ValidationException ex)
        {
            await Clients.Caller.SendAsync("EditRejected", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("EditRejected", "An unexpected error occurred while editing the message.");
        }
    }
    
    public async Task DeleteMessage(int messageId, int chatId)
    {
        var requesterId = GetUserId(Context);

        try
        {
            await mediator.Send(new DeleteMessageCommand
            {
                MessageId = messageId,
                ChatId = chatId,
                RequesterId = requesterId
            });

            await Clients.Group("Chat"+chatId).SendAsync("DeleteMessage", messageId);
        }
        catch (ValidationException ex)
        {
            await Clients.Caller.SendAsync("DeleteRejected", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("DeleteRejected", "An unexpected error occurred while deleting the message.");
        }
    }

    public async Task DeleteAttachments(List<int> attachmentsId, int chatId)
    {
        var userId = GetUserId(Context);
        
        try
        {
            var command = new DeleteAttachmentsCommand
            {
                ChatId = chatId,
                AttachmentIds = attachmentsId,
                UserId = userId
            };
            var deletedAttachmentIds = await mediator.Send(command);
            await Clients.Group("Chat" + chatId).SendAsync("AttachmentsDeleted", deletedAttachmentIds);
        }
        catch (ValidationException ex)
        {
            await Clients.Caller.SendAsync("DeleteAttachmentsRejected", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("DeleteAttachmentsRejected", "An unexpected error occurred while deleting attachments.");
        }
    }

    public async Task SendTypingStatus(int chatId, bool isTyping)
    {
        int userId = GetUserId(Context);
        string? username = GetUsername(Context);

        await Clients.OthersInGroup("Chat" + chatId).SendAsync("ReceiveTypingStatus",
            new { ChatId = chatId, UserId = userId, isTyping = isTyping, username = username });
    }

    public async Task ReadMessage(int chatId, int messageId)
    {
        int userId = GetUserId(Context);

        try
        {
            var command = new CreateMessageReactionCommand { ChatId = chatId, MessageId = messageId, ReaderId = userId };
            var msgSenderId = await mediator.Send(command);

            await Clients.Clients(_userConnections.Where(userId => userId.Key == msgSenderId).Select(p => p.Value))
                         .SendAsync("MessageSeen", new { ChatId = chatId, MessageId = messageId, ReaderId = userId });
        }
        catch (Exception ex)
        {
            // Log error, but no client notification for read
            Console.WriteLine($"Error marking message as read: {ex.Message}");
        }
    }

    public async Task ToggleEmoji(string emoji, int messageId, int chatId)
    {
        int userId = GetUserId(Context);

        try
        {
            var command = new ToggleEmojiCommand { UserId = userId, Emoji = emoji, MessageId = messageId, ChatId = chatId };
            var isCreated = await mediator.Send(command);

            await Clients.Group("Chat" + chatId).SendAsync("ReactionChange", new { isCreated = isCreated, emoji = emoji });
        }
        catch (ValidationException ex)
        {
            await Clients.Caller.SendAsync("ReactionRejected", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ReactionRejected", "An unexpected error occurred while toggling reaction.");
        }
    }
    
    public async Task ReadAllMessages(int chatId)
    {
        var userId = GetUserId(Context);

        try
        {
            var command = new ReadAllMessagesCommand { ChatId = chatId, UserId = userId };
            var readMessageIds = await mediator.Send(command);

            // Notify other users in the chat that this user has read all messages
            await Clients.OthersInGroup("Chat" + chatId).SendAsync("AllMessagesRead", 
                new { ChatId = chatId, UserId = userId, MessageIds = readMessageIds });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ReadAllRejected", "An unexpected error occurred while marking messages as read.");
            Console.WriteLine($"Error marking all messages as read: {ex.Message}");
        }
    }
    
    public async Task GetUnreadCount(int chatId)
    {
        var userId = GetUserId(Context);

        try
        {
            var query = new GetUnreadMessageCountQuery { ChatId = chatId, UserId = userId };
            var unreadCount = await mediator.Send(query);

            await Clients.Caller.SendAsync("UnreadCount", new { ChatId = chatId, Count = unreadCount });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting unread count: {ex.Message}");
        }
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
    
    private string? GetUsername(HubCallerContext context)
    {
        return context.User?.FindFirst("username")?.Value;
    }

}