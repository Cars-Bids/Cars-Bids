using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.CarSpec;

public class CarImagesObjectByCarIdSpec : Specification<CarImage, CarImage>
{
    public CarImagesObjectByCarIdSpec(int carId)
    {
        Query
            .Where(ci => ci.CarId == carId)
            .AsNoTracking();
    }
}