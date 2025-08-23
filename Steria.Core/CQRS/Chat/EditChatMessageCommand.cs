using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Chat;

public class EditChatMessageCommand : IRequest<ChatMessageDto>
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public string NewMessage { get; set; }
}

public class EditChatMessageCommandHandler(
    IMediator mediator,
    IMapper mapper,
    IGenericRepository<ChatMessage> chatMessageRepository
    ) : IRequestHandler<EditChatMessageCommand, ChatMessageDto>
{
    public async Task<ChatMessageDto> Handle(EditChatMessageCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery{ ChatId = request.ChatId, UserId = request.UserId});
        if (!isUserInChat)
            throw new HubException(Resource.UserNotParticipantOfChat);

        var messages = await chatMessageRepository.GetAsync(filter: c => c.Id == request.MessageId,
                                                                                  includeProperties: "Attachments");
        var message = messages?.FirstOrDefault()
            ?? throw new HubException(Resource.MessageDoesNotExist);

        if (message.SenderId != request.UserId)
            throw new HubException(Resource.UserNotAuthorizedToEditMessage);

        message.Message = request.NewMessage;
        await chatMessageRepository.UpdateAsync(message);

        var messageDto = mapper.Map<ChatMessageDto>(message);
        if (message.HasAttachments && message.Attachments != null)
        {
            messageDto.Attachment = message.Attachments.Select(a => a.ImageUrl).ToList();
        }

        return messageDto;
    }
}