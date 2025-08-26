using MediatR;
using Microsoft.VisualBasic;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.NotificationTypes;

public class DeleteNotificationTypeCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteNotificationTypeCommandHandler(IGenericRepository<NotificationType> repository) : IRequestHandler<DeleteNotificationTypeCommand>
{
    public async Task Handle(DeleteNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
    }
}