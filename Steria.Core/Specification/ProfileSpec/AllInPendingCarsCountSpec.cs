using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.Manager;

public class AllInPendingCarsCountSpec : CountSpec<Car>
{
    public AllInPendingCarsCountSpec()
    {
        Query
            .Where(car => car.Status == CarStatus.inPending)
            .AsNoTracking();
    }
}