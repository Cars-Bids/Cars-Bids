using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.UserNotificationSettingSpec;
using ZiggyCreatures.Caching.Fusion;

namespace Steria.Data.Services;

public class UserNotificationSettingsCacheService(IFusionCache cache,
                                                  IGenericRepository<UserNotificationSetting> repository) : IUserNotificationSettingsCacheService
{
    public async Task<List<UserNotificationSetting>> GetUserSettingsAsync(int userId)
    {
        return await cache.GetOrSetAsync<List<UserNotificationSetting>>(
            $"UserNotifSettings:{userId}", 
            async ct => await repository.GetListBySpec(new GetAllUserNotificationSettingSpec(userId), ct),
            options: new FusionCacheEntryOptions
            {
                Duration = TimeSpan.FromMinutes(60),
                FailSafeMaxDuration = TimeSpan.FromHours(1),
                JitterMaxDuration = TimeSpan.FromSeconds(10),
                AllowBackgroundDistributedCacheOperations = true
            }
        );
    }

    public async Task RemoveUserSettings(int userId)
    {
        await cache.RemoveAsync($"UserNotifSettings:{userId}");
    }
}