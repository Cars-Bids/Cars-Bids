using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ChatSpec;

namespace Steria.Core.CQRS.Chat;

public class GetChatMessagesQuery : IRequest<List<ChatMessageDto>>
{
    public int ChatId { get; set; }
    public int CurrentUserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class GetChatMessagesQueryHandler(
    IGenericRepository<ChatMessage> chatMessageRepository, 
    IMapper mapper
    ) : IRequestHandler<GetChatMessagesQuery, List<ChatMessageDto>>
{
    public async Task<List<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ChatMessagesSpec(request.ChatId, request.Page, request.PageSize);
        var messages = await chatMessageRepository.GetListBySpec(spec, cancellationToken);
        
        var res = mapper.Map<List<ChatMessageDto>>(messages, opt =>
        {
            opt.Items["UserId"] = request.CurrentUserId;
        }).ToList();

        return res;
    }
}