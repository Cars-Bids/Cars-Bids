namespace Steria.Core.DTOs;

public class UpdateUserNotificationSettingDto
{
    public string NotificationTypeKey { get; set; } = null!;
    public bool SendEmail { get; set; }
    public bool SendInSite { get; set; }
}