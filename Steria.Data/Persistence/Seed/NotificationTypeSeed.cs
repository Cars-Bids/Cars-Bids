using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Data.Persistence.Seed;

public class NotificationTypeSeed(IGenericRepository<NotificationType> repository)
{
    public async Task SeedAsync() //TODO: need to think of more notif. types and create a service to generate them
    {
        var existing = await repository.GetItemBySpec(new FirstRecordSpec<NotificationType>());

        if (existing is null)
        {
            var types = new List<NotificationType>
            {
                new NotificationType { Key = "NewAuctionInFollowedModels", Description = "Notifies when new auction appears in saved model search.", SourceType = NotificationSource.Search, RedirectRoute = "/search/make={}&model={}", DefaultSendEmail = true, DefaultSendSite = true },
                new NotificationType { Key = "NewAuctionInFollowedBrands", Description = "Notifies when new auction appears in saved make search.", SourceType = NotificationSource.Search, RedirectRoute = "/search/make={}", DefaultSendEmail = true, DefaultSendSite = true },
                new NotificationType { Key = "NewBidOnFollowedAuction", Description = "Notifies when new bid placed on an auction that is in the watchlist.", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "NewCommentOnFollowedAuction", Description = "Notifies when new comment written on an auction that is in the watchlist.", SourceType = NotificationSource.AuctionComment, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = false },
                new NotificationType { Key = "MentionedInComments", Description = "Notifies when user was mentioned in comments.", SourceType = NotificationSource.AuctionComment, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "RepliedInComments", Description = "Notifies when user was replied to in comments.", SourceType = NotificationSource.AuctionComment, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "MentionedInCommunities", Description = "Notifies when user was mentioned in communities.", SourceType = NotificationSource.CommunityComment, RedirectRoute = "/community/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "RepliedInCommunities", Description = "Notifies when user was replied in communities", SourceType = NotificationSource.CommunityComment, RedirectRoute = "/community/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "NewAuctionInFollows", Description = "Notifies when someone that user follows has a new auction", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = true, DefaultSendSite = true },
                new NotificationType { Key = "NewFollower", Description = "Notifies when someone has followed user.", SourceType = NotificationSource.Profile, RedirectRoute = "/profile/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "FollowedAuctionEndsSoon", Description = "Notifies when auction in watchlist ends in 1 hour", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = true, DefaultSendSite = true },
                new NotificationType { Key = "NewAnswerOnFollowedAuction", Description = "Notifies when questions are answered on auctions in watchlist.", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "AuctionResults", Description = "Notifies when auction ended.", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = true, DefaultSendSite = true },
                new NotificationType { Key = "Outbid", Description = "Notifies when a user's bid has been outbid.", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "LostAuction", Description = "Notifies when user lost an auction.", SourceType = NotificationSource.Auction, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "NewCommentOnAuction", Description = "Notifies when there are new comments on user's auction.", SourceType = NotificationSource.AuctionComment, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true },
                new NotificationType { Key = "NewBidOnAuction", Description = "Notifies when there are new bids on user's auction.", SourceType = NotificationSource.AuctionBid, RedirectRoute = "/auction/id={}", DefaultSendEmail = false, DefaultSendSite = true }
            };
        }
    }
}