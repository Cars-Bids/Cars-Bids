using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Data.Persistence.Repositories.Specification.CarSpec;

public class CarImagesByCarIdSpec : Specification<CarImage, string>
{
    public CarImagesByCarIdSpec(int carId)
    {
        Query
            .Where(ci => ci.CarId == carId)
            .Select(ci => ci.ImageUrl);
    }
}