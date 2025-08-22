using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.Profile;

public class UserCommentsSpec : Specification<Comment, Comment>
{
    public UserCommentsSpec(int userId, int pageNumber, int pageSize)
    {
        Query
            .Where(comment => comment.UserId == userId)
            .Include(comment => comment.Auction)
                .ThenInclude(auction => auction.Car)
                    .ThenInclude(car => car.Model)
                        .ThenInclude(model => model.Make)
            .OrderByDescending(comment => comment.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .Select(comment => comment);
    }
}