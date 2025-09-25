using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.AuctionSpec;

public class AuctionWithCarSpec : Specification<Auction, Auction>
{
    public AuctionWithCarSpec(int auctionId)
    {
        Query.Where(x => x.Id == auctionId)
            .Include(a => a.Car)
            .ThenInclude(c => c.Model)
            .ThenInclude(m => m.Make)
            .Include(a => a.Car)
            .ThenInclude(c => c.Images)
            .Include(x => x.Seller)
            .Select(x => x);
    }
}