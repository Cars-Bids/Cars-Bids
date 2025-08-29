using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Specification.СommonSpec;

namespace Steria.Core.Specification.AuctionSpec;

public class AuctionsWithCar : PagedSpec<Auction>
{
    public AuctionsWithCar(int pageNumber, int pageSize) 
        : base(pageNumber, pageSize)
    {
        Query.Include(a => a.Car)
            .ThenInclude(c => c.Model)
            .ThenInclude(m => m.Make)
            .AsNoTracking();
    }
}