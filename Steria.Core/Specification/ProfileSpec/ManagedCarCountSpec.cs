using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.CommonSpec;

namespace Steria.Core.Specification.Manager;

public class ManagedCarCountSpec : CountSpec<Car>
{
    public ManagedCarCountSpec(int userId)
    {
        Query
            .Where(car => car.Auction != null &&
                          car.ManagerId == userId)
            .AsNoTracking();
    }
}