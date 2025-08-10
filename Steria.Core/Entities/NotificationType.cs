namespace Steria.Core.Entities;

public class NotificationType
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;
    public string RedirectRoute { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<UserNotificationSetting>? UserNotificationSettings { get; set; }
    public ICollection<UserNotification>? UserNotifications { get; set; }
}