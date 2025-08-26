using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.NotificationTypeSpec;
using Steria.Core.Specification.UserNotificationSettingSpec;
using ZiggyCreatures.Caching.Fusion;

namespace Steria.Data.Services;

public class CacheService(IFusionCache cache,
                          IGenericRepository<UserNotificationSetting> settingsRepository,
                          IGenericRepository<NotificationType> notificationTypeRepository) : ICacheService
{
    public async Task<List<UserNotificationSetting>> GetUserSettingsAsync(int userId)
    {
        return await cache.GetOrSetAsync<List<UserNotificationSetting>>(
            $"UserNotifSettings:{userId}", 
            async ct => await settingsRepository.GetListBySpec(new GetAllUserNotificationSettingSpec(userId), ct),
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(60),
                FailSafeMaxDuration = TimeSpan.FromHours(1),
                JitterMaxDuration = TimeSpan.FromSeconds(10),
                AllowBackgroundDistributedCacheOperations = true
            }
        );
    }

    public async Task<NotificationType> GetNotificationTypeAsync(string key)
    {
        return await cache.GetOrSetAsync<NotificationType>(
            $"NotificationType:{key}", 
            async ct => await notificationTypeRepository.GetItemBySpec(new GetNotifTypeByKeySpec(key), ct) ?? throw new Exception("Notification type not found."),
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(60),
                FailSafeMaxDuration = TimeSpan.FromHours(1),
                JitterMaxDuration = TimeSpan.FromSeconds(10),
                AllowBackgroundDistributedCacheOperations = true
            }
        );
    }

    public async Task RemoveUserSettingsAsync(int userId)
    {
        await cache.RemoveAsync($"UserNotifSettings:{userId}");
    }

    public async Task RemoveNotificationTypeAsync(string key)
    {
        await cache.RemoveAsync($"NotificationType:{key}");
    }
}