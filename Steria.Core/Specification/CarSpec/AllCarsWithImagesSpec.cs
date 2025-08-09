using Ardalis.Specification;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.СommonSpec;

namespace CarsAndBids.Core.Specification.CarSpec;

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