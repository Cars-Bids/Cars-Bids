using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.AuctionSpec;

public class AuctionActivityBaseSpec : Specification<Auction, Auction>
{
    public AuctionActivityBaseSpec(int auctionId)
    {
        Query.Where(a => a.Id == auctionId)
            .Include(a => a.Comments)
            .ThenInclude(c => c.User)
            .Include(a => a.Bids)
            .ThenInclude(b => b.User)
            .Include(a => a.Comments)
            .ThenInclude(a => a.CommentUpvotes)
            .Select(a => a);
    }
}