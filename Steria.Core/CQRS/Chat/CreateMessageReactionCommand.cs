using MediatR;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class CreateMessageReactionCommand : IRequest<int>
{
    public int ReaderId { get; set; }
    public int ChatId { get; set; }
    public int MessageId { get; set; }
}

public class CreateMessageReactionCommandHandler(IMediator mediator,
                                                 IGenericRepository<ChatMessage> chatMessageRepository,
                                                 IGenericRepository<UserChatMessageReaction> chatMessageReactionRepository) : IRequestHandler<CreateMessageReactionCommand, int>
{
    public async Task<int> Handle(CreateMessageReactionCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery{ ChatId = request.ChatId, UserId = request.ReaderId});
        if (isUserInChat) throw new HubException("User is not a participant of this chat.");

        var spec = new GetChatMessageWithUserReactionSpec(request.ReaderId, request.MessageId);
        var msg = await chatMessageRepository.GetItemBySpec(spec);
        if (msg == null) throw new HubException("Message not found.");
        if (msg.SenderId == request.ReaderId) throw new HubException("User cant read his message.");
        if (msg.UserChatMessageReactions.FirstOrDefault() != null) throw new Exception("User already have seen this message");

        var reaction = new UserChatMessageReaction
        {
            UserId = request.ReaderId,
            ChatMessageId = request.MessageId
        };

        await chatMessageReactionRepository.InsertAsync(reaction);

        return msg.SenderId;
    }
}