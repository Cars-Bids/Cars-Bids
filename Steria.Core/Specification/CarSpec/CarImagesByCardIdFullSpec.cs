using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.CarSpec;

public class CarImagesByCardIdFullSpec : Specification<CarImage, CarImage>
{
    public CarImagesByCardIdFullSpec(int carId)
    {
        Query
            .Where(img => img.CarId == carId)
            .AsNoTracking()
            .Select(x => x);
    }
}