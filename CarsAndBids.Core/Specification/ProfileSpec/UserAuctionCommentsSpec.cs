using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.CommonSpec;

namespace CarsAndBids.Core.Specification.ProfileSpec;

public class UserAuctionCommentsSpec : PagedSpec<Comment>
{
    public UserAuctionCommentsSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(comment => comment.Auction.SellerId == userId)
            .Include(comment => comment.Auction)
            .Include(comment => comment.User)
            .AsNoTracking();

        Query.OrderByDescending(comment => comment.CreatedAt);
    }
}