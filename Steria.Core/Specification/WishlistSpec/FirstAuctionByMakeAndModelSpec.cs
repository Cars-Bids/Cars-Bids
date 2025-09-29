using Ardalis.Specification;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.WishlistSpec;
public class FirstAuctionByMakeAndModelSpec : Specification<Auction, Auction>
{
    public FirstAuctionByMakeAndModelSpec(int makeId, int? modelId)
    {
        Query
            .Where(a => a.Car.Model.MakeId == makeId)// a.Status == AuctionStatus.Active && 
            .Include(a => a.Car)
            .ThenInclude(c => c.Model)
            .ThenInclude(m => m.Make)
            .Include(a => a.Car.Images.Where(img => img.ImageCategory == ImageCategory.Main))
            .OrderBy(a => a.CreatedAt)
            .Take(1);

        if (modelId.HasValue)
        {
            Query.Where(a => a.Car.ModelId == modelId.Value);
        }

        Query.Select(a => a);
    }
}