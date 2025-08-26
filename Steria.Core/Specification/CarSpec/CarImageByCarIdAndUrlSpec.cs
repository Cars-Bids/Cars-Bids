using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.CarSpec;
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