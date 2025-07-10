using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Data.Persistence.Repositories.Specification.CarSpec;

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