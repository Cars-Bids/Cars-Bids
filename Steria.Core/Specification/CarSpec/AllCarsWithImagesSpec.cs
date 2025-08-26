using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.CarSpec;

public class AllCarsWithImagesSpec : PagedSpec<Car>
{
    public AllCarsWithImagesSpec(int pageNumber, int pageSize)
    : base(pageNumber, pageSize)
    {
        Query
            .Include(c => c.Images)
            .AsNoTracking();
    }
}