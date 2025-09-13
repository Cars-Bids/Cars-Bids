using AutoMapper;
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
                                                          IGenericRepository<UserNotificationSetting> settingRepository, IMapper mapper) : IRequestHandler<UpdateUserNotificationSettingsCommand>
{
    public async Task Handle(UpdateUserNotificationSettingsCommand request, CancellationToken cancellationToken)
    {
        var userSettings = await cacheService.GetUserSettingsAsync(request.UserId);
        var changedSettings = new List<UserNotificationSetting>();

        foreach (var dto in request.Settings)
        {
            var settingDto = userSettings.FirstOrDefault(s => s.NotificationType.Key == dto.NotificationTypeKey);
            if (settingDto is null) continue;
            if (settingDto.NotificationType.IsMandatory) continue;

            var setting = await settingRepository.GetByIdAsync(settingDto.Id);
            if (setting is null) continue;

            setting.SendEmail = dto.SendEmail;
            setting.SendInSite = dto.SendInSite;

            changedSettings.Add(setting);
        }

        await settingRepository.UpdateRangeAsync(changedSettings);
        await cacheService.RemoveUserSettingsAsync(request.UserId);
    }
}