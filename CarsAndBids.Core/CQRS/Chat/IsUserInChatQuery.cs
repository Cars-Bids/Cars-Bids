using System.Net;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Resources;
using CarsAndBids.Core.Specification.ChatSpec;
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
    public async Task<bool> Handle(IsUserInChatQuery request, CancellationToken cancellationToken)   
    {                                                                                                                   
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new HttpException(Resource.UserNotFound, HttpStatusCode.NotFound);


        var result = await chatRepository.GetItemBySpec(
            new ChatContainsUserSpec(request.ChatId, request.UserId));

        if (result == 0)
            throw new HttpException(Resource.ChatNotFoundOrUserNotInChat, HttpStatusCode.NotFound);

        return true;
    }
}