using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.CarSpec;

public class CarImagesByCarIdSpec : Specification<CarImage, string>
{
    public CarImagesByCarIdSpec(int carId)
    {
        Query
            .Where(ci => ci.CarId == carId)
            .Select(ci => ci.ImageUrl);
    }
}