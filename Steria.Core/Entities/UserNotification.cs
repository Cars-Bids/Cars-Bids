using Steria.Core.Enums;

namespace Steria.Core.Entities;

public class UserNotification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int NotificationTypeId { get; set; }
    public NotificationSource SourceType { get; set; }
    public string? CustomJsonData { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }


    public User User { get; set; } = null!;
    public NotificationType NotificationType { get; set; } = null!;
}