using MediatR;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Chat;

public class DeleteMessageCommand : IRequest
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public int RequesterId { get; set; }
}

public class DeleteMessageCommandHandler(IMediator mediator,
                                         IGenericRepository<ChatMessage> chatMessageRepository,
                                         IGenericRepository<ChatAttachment> chatAttachmentRepository,
                                         IFileService fileService) : IRequestHandler<DeleteMessageCommand>
{
    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = request.ChatId, UserId = request.RequesterId });
        if (!isUserInChat) throw new HubException("User is not a participant of this chat.");

        var messages = await chatMessageRepository.GetAsync(filter: m => m.Id == request.MessageId,
                                                     includeProperties: "Attachments");
        if (!messages.Any())
        {
            throw new HubException("Message doesn't exist.");
        }
        var message = messages.First();
        
        if (message == null) throw new HubException("Message doesn't exist.");
        if (message.SenderId != request.RequesterId) throw new HubException("User doesn't have rights to delete this message.");

        if (message.HasAttachments)
        {
            await chatAttachmentRepository.DeleteRangeAsync(message.Attachments);
            await fileService.DeleteImagesByUrlsAsync(message.Attachments.Select(url => url.ImageUrl).ToList());
            
        }

        await chatMessageRepository.DeleteAsync(message);
    } 
}