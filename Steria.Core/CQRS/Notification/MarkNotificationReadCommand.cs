using System.Net;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Notification;

public class MarkNotificationReadCommand : IRequest
{
    public int UserId { get; set; }
    public int NotificationId { get; set; }
}

public class MarkNotificationReadCommandHandler(IGenericRepository<UserNotification> notifRepository) : IRequestHandler<MarkNotificationReadCommand>
{
    public async Task Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notif = (await notifRepository.GetAsync(filter: x => x.Id == request.NotificationId, cancellationToken: cancellationToken)).FirstOrDefault();
        if (notif == null || notif.UserId != request.UserId)
            throw new HttpException("No access to this notification", HttpStatusCode.Forbidden);

        notif.IsRead = true;
        await notifRepository.UpdateAsync(notif);
    }
}