using MediatR;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class GetChatUserIdsQuery : IRequest<List<int>>
{
    public int ChatId { get; set; }
}

public class GetChatUserIdsQueryHandler(IGenericRepository<Entities.Chat> chatRepository) : IRequestHandler<GetChatUserIdsQuery, List<int>>
{
    public async Task<List<int>> Handle(GetChatUserIdsQuery request, CancellationToken cancellationToken)
    {
        var users = await chatRepository.GetItemBySpec(new ChatUsersIdsSpec(request.ChatId), cancellationToken);
        return users;
    }
}