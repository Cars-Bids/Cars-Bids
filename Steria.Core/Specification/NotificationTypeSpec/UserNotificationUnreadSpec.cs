using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.NotificationTypeSpec;

public class UserNotificationUnreadSpec : Specification<UserNotification, UserNotification>
{
    public UserNotificationUnreadSpec(int userId)
    {
        Query
            .Where(x => x.UserId == userId && !x.IsRead)
            .Include(x => x.NotificationType)
            .Select(x => x);
    }
}