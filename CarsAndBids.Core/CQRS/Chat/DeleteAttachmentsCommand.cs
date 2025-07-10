using System.Net;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace CarsAndBids.Core.CQRS.Chat;

public class DeleteAttachmentsCommand : IRequest<List<int>>
{
    public int ChatId { get; set; }
    public List<int> AttachmentIds { get; set; }
    public int UserId { get; set; }
}

public class DeleteAttachmentsCommandHandler(IMediator mediator,
                                             IFileService fileService,
                                             IGenericRepository<ChatAttachment> chatAttachmentRepository,
                                             IGenericRepository<ChatMessage> chatMessageRepository) : IRequestHandler<DeleteAttachmentsCommand, List<int>>
{
    public async Task<List<int>> Handle(DeleteAttachmentsCommand request, CancellationToken cancellationToken)
    {
        var isUserInChat = await mediator.Send(new IsUserInChatQuery { ChatId = request.ChatId, UserId = request.UserId });
        if (!isUserInChat) throw new HubException("User is not a participant of this chat.");

        var attachments = await chatAttachmentRepository.GetAsync(filter: a => request.AttachmentIds.Contains(a.Id)
                                                                                          && a.Message.ChatId == request.ChatId,
                                                                                          includeProperties: "Message");
        if (attachments.Count() != request.AttachmentIds.Count) throw new HubException("One or more attachment not found.");

        // Check if user is the sender of each message
        foreach (var chatAttachment in attachments)
        {
            var message = chatAttachment.Message;
            if (message.SenderId != request.UserId) throw new HubException($"User is not authorized to delete attachment {chatAttachment.Id}");
        }

        var deletedAttachmentIds = attachments.Select(a => a.Id).ToList();
        
        //Begin transaction
        using (var transaction = await chatAttachmentRepository.BeginTransactionAsync())
        {
            try
            {
                //Delete attachments
                await fileService.DeleteImagesByUrlsAsync(attachments.Select(u => u.ImageUrl).ToList());
                await chatAttachmentRepository.DeleteRangeAsync(attachments);

                //Update HasAttachments for messages
                var messageIds = attachments.Select(a => a.MessageId).Distinct().ToList();
                var messagesToUpdate = await chatMessageRepository.GetAsync(
                    filter: m => messageIds.Contains(m.Id) && m.HasAttachments
                );


                var messagesToUpdateList = messagesToUpdate.ToList();
                foreach (var message in messagesToUpdateList)
                {
                    var remainingAttachments =
                        await chatAttachmentRepository.GetAsync(filter: a => a.MessageId == message.Id);
                    
                    if (!remainingAttachments.Any())
                    {
                        message.HasAttachments = false;
                    }
                }
                
                if (messagesToUpdateList.Any(m => !m.HasAttachments))
                {
                    await chatMessageRepository.UpdateRangeAsync(messagesToUpdateList.Where(m => !m.HasAttachments).ToList());
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new HttpException("Failed to delete attachments.", HttpStatusCode.InternalServerError, ex);
            }
        }

        return deletedAttachmentIds;
    }
}