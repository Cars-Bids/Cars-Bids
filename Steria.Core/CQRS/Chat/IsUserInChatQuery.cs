using System.Net;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class IsUserInChatQuery : IRequest<bool>
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
}

public class IsUserInChatQueryHandler(IGenericRepository<User> userRepository,
                                      IGenericRepository<Entities.Chat> chatRepository) : IRequestHandler<IsUserInChatQuery, bool>
{
    public async Task<bool> Handle(IsUserInChatQuery request, CancellationToken cancellationToken)   
    {                                                                                                                   
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new HttpException("User not found.", HttpStatusCode.NotFound);
        }
        
        var result = await chatRepository.GetItemBySpec(
            new ChatContainsUserSpec(request.ChatId, request.UserId));

        if (result == 0)
        {
            throw new HttpException("Chat not found or user not in chat.", HttpStatusCode.NotFound);
        }

        return true;
    }
}