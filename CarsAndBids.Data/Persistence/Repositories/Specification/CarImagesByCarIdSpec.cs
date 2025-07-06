using Ardalis.Specification;
using CarsAndBids.Data.Entities;

namespace CarsAndBids.Data.Persistence.Repositories.Specification;

public class CarImagesByCarIdSpec : Specification<CarImage, string>
{
    public CarImagesByCarIdSpec(int carId)
    {
        Query
            .Where(ci => ci.CarId == carId)
            .Select(ci => ci.ImageUrl);
    }
}