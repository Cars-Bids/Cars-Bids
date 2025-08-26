using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Notifications_Custom_Data;

namespace Steria.Core.Entities;

public class UserNotification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int NotificationTypeId { get; set; }
    public string CustomDataJson { get; set; } = "{}";
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public User User { get; set; } = null!;
    public NotificationType NotificationType { get; set; } = null!;
   
    
    [NotMapped]
    public INotificationCustomData? CustomData
    {
        get => DeserializeCustomData();
        set => CustomDataJson = JsonSerializer.Serialize(value);
    }
    
    private INotificationCustomData? DeserializeCustomData()
    {
        return NotificationType?.SourceType switch
        {
            NotificationSource.Auction => JsonSerializer.Deserialize<AuctionData>(CustomDataJson),
            NotificationSource.AuctionComment => JsonSerializer.Deserialize<AuctionCommentData>(CustomDataJson),
            _ => null
        };
    }
}