using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.AuctionSpec;

public class AuctionSellerCommentsSpec : PagedSpec<Comment>
{
    public AuctionSellerCommentsSpec(int auctionId, int sellerId, int pageNumber, int pageSize) 
        : base(pageNumber, pageSize)
    {
        Query.Where(c => c.AuctionId == auctionId && c.UserId == sellerId)
            .OrderByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id)
            .Include(c => c.User);
    }
}