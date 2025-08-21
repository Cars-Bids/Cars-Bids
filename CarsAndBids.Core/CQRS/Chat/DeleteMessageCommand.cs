using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.CQRS.Chat;

public class DeleteMessageCommand : IRequest
{
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public int RequesterId { get; set; }
}

public class DeleteMessageCommandHandler(
    IMediator mediator,
    IGenericRepository<ChatMessage> chatMessageRepository,
    IGenericRepository<ChatAttachment> chatAttachmentRepository,
    IFileService fileService
    ) : IRequestHandler<DeleteMessageCommand>
{
    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = request.ChatId, UserId = request.RequesterId });
        if (!isUserInChat)
            throw new HubException(Resource.UserNotParticipantOfChat);

        var messages = await chatMessageRepository.GetAsync(filter: m => m.Id == request.MessageId,
                                                     includeProperties: "Attachments");

        var message = messages?.FirstOrDefault()
            ?? throw new HubException(Resource.MessageDoesNotExist);

        if (message.SenderId != request.RequesterId)
            throw new HubException(Resource.UserNotAuthorizedToDeleteMessage);

        if (message.HasAttachments)
        {
            await chatAttachmentRepository.DeleteRangeAsync(message.Attachments!);
            await fileService.DeleteImagesByUrlsAsync(message.Attachments!.Select(url => url.ImageUrl).ToList());            
        }

        await chatMessageRepository.DeleteAsync(message);
    } 
}