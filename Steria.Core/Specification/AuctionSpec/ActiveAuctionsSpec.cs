using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.AuctionSpec;

public class ActiveAuctionsSpec : Specification<Auction, Auction>
{
    public ActiveAuctionsSpec(int count)
    {
        Query
            .Where(a => a.Status == AuctionStatus.Active)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .Include(a => a.Car)
                .ThenInclude(c => c.Images
                    .Where(img => img.ImageCategory == ImageCategory.Main || img.ImageCategory == ImageCategory.Exterior))
            .Include(a => a.Car)
                .ThenInclude(c => c.Model)
                    .ThenInclude(m => m.Make)
            .Select(a => a);
    }
}