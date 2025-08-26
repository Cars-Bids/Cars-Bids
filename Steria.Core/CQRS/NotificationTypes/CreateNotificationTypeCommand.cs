using AutoMapper;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.NotificationTypes;

public class CreateNotificationTypeCommand : IRequest
{
    public string Key { get; set; }
    public string RedirectRoute { get; set; }
    public string Description { get; set; }
    public NotificationSource SourceType { get; set; }
    public bool DefaultSendEmail { get; set; }
    public bool DefaultSendSite { get; set; }
}

public class CreateNotificationTypeCommandHandler(IMapper mapper,
                                                  IGenericRepository<NotificationType> repository) : IRequestHandler<CreateNotificationTypeCommand>
{
    public async Task Handle(CreateNotificationTypeCommand cmd, CancellationToken cancellationToken)
    {
        var res = mapper.Map<NotificationType>(cmd);

        await repository.InsertAsync(res);
    }
}