using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.AuctionSpec;

public class AuctionMostUpvotedCommentsSpec : PagedSpec<Comment>
{
    public AuctionMostUpvotedCommentsSpec(int auctionId, int pageNumber, int pageSize) 
        : base(pageNumber, pageSize)
    {
        Query.Where(c => c.AuctionId == auctionId)
            .OrderByDescending(c => c.CommentUpvotes.Count)
            .ThenByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id)
            .Include(c => c.User);
    }
}