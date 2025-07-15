using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.CarSpec;

public class AllCarsWithImagesSpec : Specification<Car, Car>
{
    public AllCarsWithImagesSpec()
    {
        Query
            .Include(c => c.Images)
            .AsNoTracking();

        Query.Select(c => c);
    }
}