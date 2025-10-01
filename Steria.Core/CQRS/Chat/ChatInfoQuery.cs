using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class ChatInfoQuery : IRequest<ChatInfoDto>
{
    public int ChatId { get; set; }
    public int CurentUser { get; set; }
}

public class ChatInfoQueryHandler(IGenericRepository<Entities.Chat> chatRepository) : IRequestHandler<ChatInfoQuery, ChatInfoDto>
{
    public async Task<ChatInfoDto> Handle(ChatInfoQuery request, CancellationToken cancellationToken)
    {
        var spec = new ChatInfoSpec(request.ChatId, request.CurentUser);
        var dto = await chatRepository.GetItemBySpec(spec, cancellationToken);

        return dto;
    }
}