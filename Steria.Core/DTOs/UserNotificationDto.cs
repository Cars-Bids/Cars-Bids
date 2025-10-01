using Steria.Core.Interfaces;

namespace Steria.Core.DTOs;

public class UserNotificationDto
{
    public int Id { get; set; }
    public string TypeKey { get; set; } = null!;
    public string RedirectRoute { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CustomData { get; set; }
}