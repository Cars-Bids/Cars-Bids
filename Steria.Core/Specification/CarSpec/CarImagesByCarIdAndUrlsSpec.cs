using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.CarSpec;

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