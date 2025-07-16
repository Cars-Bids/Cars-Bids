using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.ChatSpec;
using MediatR;

namespace CarsAndBids.Core.CQRS.Chat;

public class GetChatMessagesQuery : IRequest<List<ChatMessageDto>>
{
    public int ChatId { get; set; }
    public int CurrentUserId { get; set; }
}

public class GetChatMessagesQueryHandler(IGenericRepository<ChatMessage> chatMessageRepository, 
                                            IMapper mapper) : IRequestHandler<GetChatMessagesQuery, List<ChatMessageDto>>
{
    public async Task<List<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllChatMessagesSpec(request.ChatId);
        var messages = await chatMessageRepository.GetListBySpec(spec);
        
        var res = mapper.Map<List<ChatMessageDto>>(messages, opt =>
        {
            opt.Items["UserId"] = request.CurrentUserId;
        }).ToList();

        return res;
    }
}