using Steria.Core.Interfaces;

namespace Steria.Core.Notifications_Custom_Data;

public class AuctionData : INotificationCustomData
{
    public int AuctionId { get; set; }
}