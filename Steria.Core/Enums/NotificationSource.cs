namespace Steria.Core.Enums;

public enum NotificationSource //TODO: need to stack notifications types such as new bid, new comments; so they would look like "5 new comments"/"2 new bids". Create a custom data for these sources
{
    Auction,
    AuctionBid,
    AuctionComment,
    Profile,
    Search,
    Community,
    CommunityComment,
    None
}