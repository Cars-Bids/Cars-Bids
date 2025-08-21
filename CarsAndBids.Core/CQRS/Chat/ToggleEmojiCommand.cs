using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Resources;
using CarsAndBids.Core.Specification.ChatSpec;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace CarsAndBids.Core.CQRS.Chat;

public class ToggleEmojiCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public string Emoji { get; set; } = null!;
}

public class ToggleEmojiCommandHandler(
    IMediator mediator,
    IGenericRepository<UserChatMessageReaction> messageReactionRepository
    ) : IRequestHandler<ToggleEmojiCommand, bool>
{
    public async Task<bool> Handle(ToggleEmojiCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery{ ChatId = request.ChatId, UserId = request.UserId});
        if (isUserInChat)
            throw new HubException(Resource.UserNotInChat);

        var spec = new GetChatMessageReactionSpec(request.UserId, request.MessageId);
        var reaction = await messageReactionRepository.GetItemBySpec(spec, cancellationToken);

        bool isCreated;
        
        if (reaction != null)
        {
            var existing = reaction.EmojiReactions?.FirstOrDefault(x => x.Emoji == request.Emoji);
            if (existing != null)
            {
                reaction.EmojiReactions!.Remove(existing); //toggle off
                isCreated = false;
            } 
            else
            {
                reaction.EmojiReactions?.Add(new EmojiReaction { Emoji = request.Emoji }); //toggle on
                isCreated = true;
            }
            await messageReactionRepository.SaveAsync();
        }
        else 
            throw new HubException(Resource.EmojiToggleFailed);

        return isCreated;
    }
}