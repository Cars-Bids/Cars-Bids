using AutoMapper;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.NotificationTypes;

public class UpdateNotificationTypeCommand : IRequest
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public string RedirectRoute { get; set; } = null!;
    public NotificationSource SourceType { get; set; }
    public string Description { get; set; } = null!;
    public bool DefaultSendEmail { get; set; }
    public bool DefaultSendSite { get; set; }
}

public class UpdateNotificationTypeCommandHandler(IMapper mapper,
                                                  IGenericRepository<NotificationType> repository) : IRequestHandler<UpdateNotificationTypeCommand>
{
    public async Task Handle(UpdateNotificationTypeCommand cmd, CancellationToken cancellationToken)
    {
        var existingType = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingType);

        await repository.UpdateAsync(existingType!);
    }
}