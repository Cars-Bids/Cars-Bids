using Steria.Core.Enums;

namespace Steria.Core.Entities;

public class NotificationType
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public string RedirectRoute { get; set; } = null!;
    public NotificationSource SourceType { get; set; }
    public string Description { get; set; } = null!;
    public bool DefaultSendEmail { get; set; } = false;
    public bool DefaultSendSite { get; set; } = false;

    public ICollection<UserNotificationSetting>? UserNotificationSettings { get; set; }
    public ICollection<UserNotification>? UserNotifications { get; set; }
}