using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.UserNotificationSettingSpec;

public class GetAllUserNotificationSettingSpec : Specification<UserNotificationSetting, UserNotificationSetting>
{
    public GetAllUserNotificationSettingSpec(int userId)
    {
        Query.Where(n => n.UserId == userId)
             .Include(n => n.NotificationType);
    }
}