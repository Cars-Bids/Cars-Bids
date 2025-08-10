using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Chat;

public class SendChatMessageCommand : IRequest<ChatMessageDto>
{
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public string? Message { get; set; }
    public List<IFormFile>? Attachments { get; set; }
}

public class SendChatMessageCommandHandler(IGenericRepository<Entities.Chat> chatRepository,
                                           IGenericRepository<User> userRepository,
                                           IGenericRepository<ChatMessage> chatMessageRepository,
                                           IGenericRepository<ChatAttachment> chatAttachmentRepository,
                                           IFileService fileService,
                                           IMediator mediator,
                                           IMapper mapper) : IRequestHandler<SendChatMessageCommand, ChatMessageDto>
{
    public async Task<ChatMessageDto> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = request.ChatId, UserId = request.SenderId });
        if (!isUserInChat)
        {
            throw new HubException("User is not a participant of this chat.");
        }

        var message = mapper.Map<ChatMessage>(request);
        message.HasAttachments = request.Attachments != null && request.Attachments.Any();
        
        await chatMessageRepository.InsertAsync(message);

        var newMessage = mapper.Map<ChatMessageDto>(message);
        
        if (request.Attachments != null && request.Attachments.Any())
        {
            var attachmentUrls = await fileService.UploadImagesAsync(request.Attachments);
            var attachments = attachmentUrls.Select(url => new ChatAttachment
            {
                MessageId = message.Id,
                ImageUrl = url
            }).ToList();

            await chatAttachmentRepository.InsertRangeAsync(attachments);
            
            newMessage.Attachment = attachmentUrls;
        }

        return newMessage;
    }
}