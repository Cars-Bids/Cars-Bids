using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.ProfileSpec;

public class UserAuctionCommentsCountSpec : CountSpec<Comment>
{
    public UserAuctionCommentsCountSpec(int userId)
    {
        Query
            .Where(comment => comment.Auction.SellerId == userId)
            .AsNoTracking();
    }
}