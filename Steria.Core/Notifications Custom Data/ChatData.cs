using Steria.Core.Interfaces;

namespace Steria.Core.Notifications_Custom_Data;

public class ChatData : INotificationCustomData
{
    public int ChatId { get; set; }
    public string AuctionTitle { get; set; }
}