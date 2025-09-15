using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.ProfileSpec;

public class UserAuctionCommentsSpec : PagedSpec<Comment>
{
    public UserAuctionCommentsSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(comment => comment.Auction.SellerId == userId)
            .Include(comment => comment.Auction)
                .ThenInclude(auction => auction.Car)
                    .ThenInclude(car => car.Model)
                        .ThenInclude(model => model.Make)
            .Include(comment => comment.Auction)
                .ThenInclude(auction => auction.Car)
                    .ThenInclude(car => car.Images)
            .Include(comment => comment.User)
            .AsNoTracking();

        Query.OrderByDescending(comment => comment.CreatedAt);
    }
}