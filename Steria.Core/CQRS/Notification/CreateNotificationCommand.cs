using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.NotificationTypeSpec;

namespace Steria.Core.CQRS.Notification;

public class CreateNotificationCommand : IRequest
{
    public int UserId { get; set; }
    public string NotificationTypeKey { get; set; } = null!;
    public INotificationCustomData CustomData { get; set; } = null!;
}

public class CreateNotificationCommandHandler(IGenericRepository<NotificationType> notifTypeRepository,
                                              IMediator mediator,
                                              IGenericRepository<UserNotification> userNotifRepository) : IRequestHandler<CreateNotificationCommand>
{
    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notifType = await notifTypeRepository.GetItemBySpec(new GetNotifTypeByKeySpec(request.NotificationTypeKey), cancellationToken);
        
        if (notifType == null)
            throw new Exception("NotificationType not found");
        
        
    }
}