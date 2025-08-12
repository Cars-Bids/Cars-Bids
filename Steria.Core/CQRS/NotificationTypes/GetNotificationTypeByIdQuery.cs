using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.NotificationTypes;

public class GetNotificationTypeByIdQuery : IRequest<NotificationTypeDto?>
{
    public int Id { get; set; }
}

public class GetNotificationTypeByIdQueryHandler(IGenericRepository<NotificationType> repository,
                                                 IMapper mapper) : IRequestHandler<GetNotificationTypeByIdQuery, NotificationTypeDto?>
{
    public async Task<NotificationTypeDto?> Handle(GetNotificationTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return mapper.Map<NotificationTypeDto>(await repository.GetByIdAsync(request.Id));
    }
}