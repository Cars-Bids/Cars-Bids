using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.NotificationTypeSpec;

namespace Steria.Core.CQRS.Notification;

public class GetUnreadNotificationsQuery : IRequest<List<UserNotificationDto>>
{
    public int UserId { get; set; }
}

public class GetUnreadNotificationsQueryHandler(IGenericRepository<UserNotification> notificationRepository,
                                                IMapper mapper) : IRequestHandler<GetUnreadNotificationsQuery, List<UserNotificationDto>>
{
    public async Task<List<UserNotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserNotificationUnreadSpec(request.UserId);
        var notifs = await notificationRepository.GetListBySpec(spec, cancellationToken);

        return mapper.Map<List<UserNotificationDto>>(notifs);
    }
}