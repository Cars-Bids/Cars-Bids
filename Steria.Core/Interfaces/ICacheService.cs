using Steria.Core.DTOs;
using Steria.Core.Entities;

namespace Steria.Core.Interfaces;

public interface ICacheService
{
    Task<List<UserNotificationSettingDto>> GetUserSettingsAsync(int userId);
    Task<NotificationType> GetNotificationTypeAsync(string key);
    Task RemoveUserSettingsAsync(int userId);
    Task RemoveNotificationTypeAsync(string key);
}