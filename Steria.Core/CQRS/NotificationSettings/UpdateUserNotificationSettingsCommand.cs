using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.NotificationSettings;

public class UpdateUserNotificationSettingsCommand : IRequest
{
    public int UserId { get; set; }
    public List<UpdateUserNotificationSettingDto> Settings { get; set; }
}

public class UpdateUserNotificationSettingsCommandHandler(ICacheService cacheService,
                                                          IGenericRepository<UserNotificationSetting> settingRepository) : IRequestHandler<UpdateUserNotificationSettingsCommand>
{
    public async Task Handle(UpdateUserNotificationSettingsCommand request, CancellationToken cancellationToken)
    {
        var userSettings = await cacheService.GetUserSettingsAsync(request.UserId);
        var changedSettings = new List<UserNotificationSetting>();
        foreach (var dto in request.Settings)
        {
            var setting = userSettings.FirstOrDefault(s => s.NotificationType.Key == dto.NotificationTypeKey);

            if (setting is null) continue;
            if (setting.NotificationType.IsMandatory) continue;

            setting.SendEmail = dto.SendEmail;
            setting.SendInSite = dto.SendInSite;
            
            changedSettings.Add(setting);
        }

        await settingRepository.UpdateRangeAsync(changedSettings);
        await cacheService.RemoveUserSettingsAsync(request.UserId);
    }
}