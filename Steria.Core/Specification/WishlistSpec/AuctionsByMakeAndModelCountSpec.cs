using Ardalis.Specification;
using Steria.Core.Entities;

namespace Steria.Core.Specification.WishlistSpec;

public class AuctionsByMakeAndModelCountSpec : Specification<Auction>
{
    public AuctionsByMakeAndModelCountSpec(int makeId, int? modelId)
    {
        Query
            .Where(a => a.Car.Model.MakeId == makeId && (modelId == null || a.Car.ModelId == modelId))
            .AsNoTracking();
    }
}