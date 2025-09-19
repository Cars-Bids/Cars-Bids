using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.CarSpec;

public class CarImagesByCategorySpec : Specification<CarImage, CarImage>
{
    public CarImagesByCategorySpec(int carId, ImageCategory category)
    {
        Query
            .Where(img => img.CarId == carId && img.ImageCategory == category)
            .OrderBy(img => img.OrderNumber)
            .Select(x => x);
        
    }
}