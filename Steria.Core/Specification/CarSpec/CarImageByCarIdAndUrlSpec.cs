using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.CarSpec;
public class CarImageByCarIdAndUrlSpec : Specification<CarImage, CarImage>
{
    public CarImageByCarIdAndUrlSpec(int carId, string imageUrl)
    {
        Query
            .Where(img => img.CarId == carId && img.ImageUrl == imageUrl)
            .AsNoTracking();

        Query.Select(img => img);
    }
}