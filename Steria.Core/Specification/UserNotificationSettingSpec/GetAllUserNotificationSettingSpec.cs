using Ardalis.Specification;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.UserNotificationSettingSpec;

public class GetAllUserNotificationSettingSpec : Specification<UserNotificationSetting, UserNotificationSettingDto>
{
    public GetAllUserNotificationSettingSpec(int userId)
    {
        Query.Where(n => n.UserId == userId)
             .Include(n => n.NotificationType)
             .Select(n => new UserNotificationSettingDto
             {
                 Id = n.Id,
                 UserId = n.UserId,
                 NotificationTypeId = n.NotificationTypeId,
                 SendEmail = n.SendEmail,
                 SendInSite = n.SendInSite,
                 NotificationType = new NotificationTypeDto
                 {
                     Id = n.NotificationType.Id,
                     Key = n.NotificationType.Key,
                     RedirectRoute = n.NotificationType.RedirectRoute,
                     SourceType = n.NotificationType.SourceType,
                     Description = n.NotificationType.Description,
                     //DefaultSendEmail = n.NotificationType.DefaultSendEmail,
                     //DefaultSendSite = n.NotificationType.DefaultSendSite,
                     IsMandatory = n.NotificationType.IsMandatory
                 }
             });
    }
}