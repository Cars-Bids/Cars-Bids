using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Resources;
using CarsAndBids.Core.Specification.ChatSpec;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace CarsAndBids.Core.CQRS.Chat;

public class CreateMessageReactionCommand : IRequest<int>
{
    public int ReaderId { get; set; }
    public int ChatId { get; set; }
    public int MessageId { get; set; }
}

public class CreateMessageReactionCommandHandler(
    IMediator mediator,
    IGenericRepository<ChatMessage> chatMessageRepository,
    IGenericRepository<UserChatMessageReaction> chatMessageReactionRepository
    ) : IRequestHandler<CreateMessageReactionCommand, int>
{
    public async Task<int> Handle(CreateMessageReactionCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery{ ChatId = request.ChatId, UserId = request.ReaderId});
        if (isUserInChat)
            throw new HubException(Resource.UserNotParticipantOfChat);

        var spec = new GetChatMessageWithUserReactionSpec(request.ReaderId, request.MessageId);
        
        var msg = await chatMessageRepository.GetItemBySpec(spec)
            ?? throw new HubException(Resource.MessageNotFound);
        
        if (msg.SenderId == request.ReaderId)
            throw new HubException(Resource.UserCannotReadOwnMessage);

        if (msg.UserChatMessageReactions?.FirstOrDefault() != null)
            throw new Exception(Resource.UserAlreadySeenMessage);

        var reaction = new UserChatMessageReaction
        {
            UserId = request.ReaderId,
            ChatMessageId = request.MessageId
        };

        await chatMessageReactionRepository.InsertAsync(reaction);

        return msg.SenderId;
    }
}