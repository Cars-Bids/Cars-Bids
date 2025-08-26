using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.NotificationTypes;

public class GetAllNotificationTypesQuery : IRequest<List<NotificationTypeDto>>
{
    
}

public class GetAllNotificationTypesQueryHandler(IGenericRepository<NotificationType> repository,
                                                 IMapper mapper) : IRequestHandler<GetAllNotificationTypesQuery, List<NotificationTypeDto>>
{
    public async Task<List<NotificationTypeDto>> Handle(GetAllNotificationTypesQuery request, CancellationToken cancellationToken)
    {
        var notif = await repository.GetAsync(cancellationToken: cancellationToken);
        
        return mapper.Map<List<NotificationTypeDto>>(notif);
    }
}