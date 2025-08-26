using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.Profile;

public class UserInReviewCarsSpec : PagedSpec<Car>
{
    public UserInReviewCarsSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(car => car.OwnerId == userId && car.Status == CarStatus.inReview)
            .Include(car => car.Model)
            .ThenInclude(model => model.Make)
            .Include(car => car.BodyStyle)
            .Include(car => car.Images.Where(img => img.ImageCategory == ImageCategory.Main).Take(1))
            .AsNoTracking();

        Query.OrderByDescending(car => car.CreatedAt);
    }
}