using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.Profile;

public class UserInReviewCarsCountSpec : CountSpec<Car>
{
    public UserInReviewCarsCountSpec(int userId)
    {
        Query
            .Where(car => car.OwnerId == userId &&
                (
                    //car.Status == CarStatus.inReview
                    //|| car.Status == CarStatus.inPending
                    //|| car.Auction.Status == AuctionStatus.Pending
                    //|| car.Auction.Status == AuctionStatus.New
                    //|| car.Auction.Status == AuctionStatus.Approved
                    (car.Status == CarStatus.inReview
                    || car.Status == CarStatus.inPending
                    || (car.Auction.Status == AuctionStatus.Pending && car.Status == CarStatus.Approved))
                    && car.Auction.Status != AuctionStatus.Active
                ))
            .AsNoTracking();
    }
}