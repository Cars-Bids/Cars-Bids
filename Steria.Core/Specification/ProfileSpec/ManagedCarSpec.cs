using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.Manager;

public class ManagedCarSpec : PagedSpec<Car>
{
    public ManagedCarSpec(int userId, int pageNumber, int pageSize)
        : base(pageNumber, pageSize)
    {
        Query
            .Where(car => car.Auction != null &&
                          car.ManagerId == userId)
            .Include(car => car.Model)
                .ThenInclude(model => model.Make)
            .Include(car => car.BodyStyle)
            .Include(car => car.Images.Where(img => img.ImageCategory == ImageCategory.Main).Take(1))
            .Include(car => car.Auction)
            .AsNoTracking();

        Query.OrderByDescending(car => car.CreatedAt);
    }
}