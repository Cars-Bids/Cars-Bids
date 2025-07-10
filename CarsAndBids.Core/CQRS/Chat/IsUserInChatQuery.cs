using System.Net;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Chat;

public class IsUserInChatQuery : IRequest<bool>
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
}

public class IsUserInChatQueryHandler(IGenericRepository<User> userRepository,
                                      IGenericRepository<Entities.Chat> chatRepository) : IRequestHandler<IsUserInChatQuery, bool>
{
    public async Task<bool> Handle(IsUserInChatQuery request, CancellationToken cancellationToken)                         // TODO: needs to be reworked
    {                                                                                                                      // (retrieving list when using only 1 object)
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new HttpException("User not found.", HttpStatusCode.NotFound);
        }
        
        var chat = await chatRepository.GetAsync(
            filter: c => c.Id == request.ChatId,
            includeProperties: "Participants"
        );

        if (!chat.Any())
        {
            throw new HttpException("Chat not found.", HttpStatusCode.NotFound);
        }
        
        var isUserInChat = chat.First().Participants?.Any(p => p.Id == request.UserId) ?? false;
        
        return isUserInChat;
    }
}