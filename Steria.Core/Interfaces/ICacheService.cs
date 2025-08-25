using Steria.Core.Entities;

namespace Steria.Core.Interfaces;

public interface IUserNotificationSettingsCacheService
{
    Task<List<UserNotificationSetting>> GetUserSettingsAsync(int userId);
    Task RemoveUserSettings(int userId);
}