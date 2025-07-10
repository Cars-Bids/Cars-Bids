using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Queries.Chat;

public class GetChatMessagesQuery : IRequest<List<ChatMessageDto>>
{
    public int ChatId { get; set; }
}

public class GetChatMessagesQueryHandler(IGenericRepository<ChatMessage> chatMessageRepository, 
                                            IMapper mapper) : IRequestHandler<GetChatMessagesQuery, List<ChatMessageDto>>
{
    public async Task<List<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await chatMessageRepository.GetAsync(filter: m => m.ChatId == request.ChatId,
                                                                               includeProperties: "Attachments");
        return mapper.Map<List<ChatMessageDto>>(messages.ToList());
    }
}