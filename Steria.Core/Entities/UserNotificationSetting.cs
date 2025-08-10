namespace Steria.Core.Entities;

public class UserNotificationSetting
{
    public int Id { get; set; }
    public int NotificationTypeId { get; set; }
    public int UserId { get; set; }
    public bool SendEmail { get; set; }
    public bool SendInSite { get; set; }

    public User User { get; set; } = null!;
    public NotificationType NotificationType { get; set; } = null!;
}