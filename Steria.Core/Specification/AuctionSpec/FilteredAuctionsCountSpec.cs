using Ardalis.Specification;
using Steria.Core.CQRS.Auctions;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Specification.AuctionSpec;

public class FilteredAuctionsCountSpec : Specification<Auction>
{
    public FilteredAuctionsCountSpec(GetFilteredAuctionsQuery query)
    {
        //Query.Where(a => a.Status == AuctionStatus.Active);
        Query.Where(a =>
            a.Status == AuctionStatus.Active ||
            a.Status == AuctionStatus.Pending ||
            a.Status == AuctionStatus.NotSold ||
            a.Status == AuctionStatus.Sold
        );


        if (!string.IsNullOrWhiteSpace(query.Transmission))
        {
            if (Enum.TryParse<TransmissionType>(query.Transmission, true, out var transmissionType))
            {
                Query.Where(a => a.Car.TransmissionType == transmissionType);
            }
        }

        if (!string.IsNullOrWhiteSpace(query.BodyStyle))
        {
            Query.Where(a => a.Car.BodyStyle.StyleName == query.BodyStyle);
        }

        if (query.MinMileage.HasValue)
        {
            Query.Where(a => a.Car.Mileage >= query.MinMileage.Value);
        }
        if (query.MaxMileage.HasValue)
        {
            Query.Where(a => a.Car.Mileage <= query.MaxMileage.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.MakeModelSearch))
        {
            var searchTerms = query.MakeModelSearch.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var term in searchTerms)
            {
                Query.Where(a => a.Car.Model.Name.ToLower().Contains(term) ||
                                a.Car.Model.Make.Name.ToLower().Contains(term));
            }
        }

        Query.AsNoTracking();
    }
}