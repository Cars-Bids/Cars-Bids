using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.CarSpec;

public class CarImagesMaxOrderNumberSpec : Specification<CarImage, int>
{
    public CarImagesMaxOrderNumberSpec(int carId)
    {
        Query
            .Where(img => img.CarId == carId)
            .AsNoTracking();

        Query.Select(img => img.OrderNumber);
    }
}