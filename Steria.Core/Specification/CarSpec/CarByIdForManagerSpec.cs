using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.CarSpec;

public class CarByIdForManagerSpec : Specification<Car, Car>
{
    public CarByIdForManagerSpec(int carId)
    {
        Query
            .Where(c => c.Id == carId)
            .Include(c => c.Owner)
            .Include(c => c.BodyStyle)
            .Include(c => c.Model)
                .ThenInclude(m => m.Make)
            .Include(c => c.Images)
            .AsNoTracking()
            .Select(c => c);
    }
}