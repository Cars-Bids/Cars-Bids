using Ardalis.Specification;
using CarsAndBids.Core.Entities;

namespace CarsAndBids.Core.Specification.CarSpec;

public class CarImagesByCarIdAndUrlsSpec : Specification<CarImage, CarImage>
{
    public CarImagesByCarIdAndUrlsSpec(int carId, List<string> imageUrls)
    {
        Query
            .Where(img => img.CarId == carId && imageUrls.Contains(img.ImageUrl!))
            .AsNoTracking();

        Query.Select(img => img);
    }
}