using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class ReadAllMessagesCommand : IRequest<List<int>>
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
}

public class ReadAllMessagesCommandHandler(IGenericRepository<ChatMessage> messageRepository,
                                           IGenericRepository<UserChatMessageReaction> reactionRepository) : IRequestHandler<ReadAllMessagesCommand, List<int>>
{
    public async Task<List<int>> Handle(ReadAllMessagesCommand request, CancellationToken cancellationToken)
    {
        var spec = new ChatUnreadMessageSpec(request.ChatId, request.UserId);
        var unreadMessages = await messageRepository.GetListBySpec(spec, cancellationToken);

        var readReactions = unreadMessages.Select(id => new UserChatMessageReaction
        {
            ChatMessageId = request.ChatId,
            UserId = id,
            SeenAt = DateTime.UtcNow
        }).ToList();
        
        if (readReactions.Any())
        {
            await reactionRepository.InsertRangeAsync(readReactions);
        }

        return unreadMessages;
    }
}