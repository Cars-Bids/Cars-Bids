using Steria.Core.Interfaces;

namespace Steria.Core.Notifications_Custom_Data;

public class AuctionCommentData : INotificationCustomData
{
    public int AuctionId { get; set; }
    public int CommentId { get; set; }
}