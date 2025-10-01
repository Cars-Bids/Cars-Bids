using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class GetUnreadMessageCountQuery : IRequest<int>
{
    public int ChatId { get; set; }
    public int UserId { get; set; }
}

public class GetUnreadMessageCountQueryHandler(IGenericRepository<ChatMessage> messageRepository) : IRequestHandler<GetUnreadMessageCountQuery, int>
{
    public async Task<int> Handle(GetUnreadMessageCountQuery request, CancellationToken cancellationToken)
    {
        var spec = new UnreadMessagesCountSpec(request.ChatId, request.UserId);
        return await messageRepository.CountAsync(spec, cancellationToken);
    }
}