using System.Net.Http.Headers;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Steria.Core.DTOs;
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
                                              IGenericRepository<UserNotification> userNotifRepository,
                                              ICacheService cacheService,
                                              IRealtimeNotifier notifier,
                                              IMapper mapper) : IRequestHandler<CreateNotificationCommand>
{
    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notifType = await cacheService.GetNotificationTypeAsync(request.NotificationTypeKey);
        
        if (notifType == null)
            throw new Exception("NotificationType not found");

        var settings = await cacheService.GetUserSettingsAsync(request.UserId);
        var typeSetting = settings.FirstOrDefault(n => n.NotificationType.Key == request.NotificationTypeKey);

        if (typeSetting is { SendEmail: false, SendInSite: false }) return; // user disabled this notification fully
        
        if (typeSetting!.SendInSite)
        {
            var notification = new UserNotification
            {
                UserId = request.UserId,
                NotificationTypeId = notifType.Id,
                CustomDataJson = JsonSerializer.Serialize(request.CustomData, request.CustomData.GetType())
            };

            await userNotifRepository.InsertAsync(notification);
            await notifier.SendToUserAsync(request.UserId, notifType.SourceType, mapper.Map<UserNotificationDto>(notification));
        }

        if (typeSetting.SendEmail)
        {
            //TODO: add email logic
        }
    }
}