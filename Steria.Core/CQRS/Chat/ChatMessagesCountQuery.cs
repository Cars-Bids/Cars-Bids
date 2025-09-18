using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.ChatSpec;

public class ChatMessagesCountQuery : IRequest<int>
{
    public int ChatId { get; set; }
}

public class GetChatMessagesCountQueryHandler(IGenericRepository<ChatMessage> messageRepository) : IRequestHandler<ChatMessagesCountQuery, int>
{

    public async Task<int> Handle(ChatMessagesCountQuery request, CancellationToken cancellationToken)
    {
        var spec = new ChatMessagesCountSpec(request.ChatId);
        return await messageRepository.CountAsync(spec, cancellationToken);
    }
}