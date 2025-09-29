using Ardalis.Specification;
using Steria.Core.CQRS.Auctions;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Specification.CommonSpec;
using Steria.Core.Specification.СommonSpec;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Steria.Core.Specification.AuctionSpec;

public class FilteredAuctionsSpec : PagedSpec<Auction>
{
    public FilteredAuctionsSpec(GetFilteredAuctionsQuery query)
        : base(query.PageNumber, query.PageSize)
    {
        Query.Include(a => a.Car)
            .ThenInclude(c => c.Model)
            .ThenInclude(m => m.Make)
            .Include(a => a.Car)
            .ThenInclude(c => c.Images.Where(img => img.ImageCategory == ImageCategory.Main))
            .Include(a => a.Car)
            .ThenInclude(c => c.BodyStyle)
            .AsNoTracking();

        // Фільтрація по статусу (активні аукціони)
        //Query.Where(a => a.Status == AuctionStatus.Active);

        // Фільтрація по Transmission
        if (!string.IsNullOrWhiteSpace(query.Transmission))
        {
            if (Enum.TryParse<TransmissionType>(query.Transmission, true, out var transmissionType))
            {
                Query.Where(a => a.Car.TransmissionType == transmissionType);
            }
        }

        // Фільтрація по BodyStyle
        if (!string.IsNullOrWhiteSpace(query.BodyStyle))
        {
            Query.Where(a => a.Car.BodyStyle.StyleName == query.BodyStyle);
        }

        // Фільтрація по Mileage
        if (query.MinMileage.HasValue)
        {
            Query.Where(a => a.Car.Mileage >= query.MinMileage.Value);
        }
        if (query.MaxMileage.HasValue)
        {
            Query.Where(a => a.Car.Mileage <= query.MaxMileage.Value);
        }

        // Фільтрація по Make/Model
        if (!string.IsNullOrWhiteSpace(query.MakeModelSearch))
        {
            var searchTerms = query.MakeModelSearch.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var term in searchTerms)
            {
                Query.Where(a => a.Car.Model.Name.ToLower().Contains(term) ||
                                a.Car.Model.Make.Name.ToLower().Contains(term));
            }
        }

        // Сортування
        if (string.IsNullOrWhiteSpace(query.SortBy) || query.SortBy == "CreatedAt")
        {
            if (query.SortDescending)
                Query.OrderByDescending(a => a.CreatedAt);
            else
                Query.OrderBy(a => a.CreatedAt);
        }
        else if (query.SortBy == "Year")
        {
            if (query.SortDescending)
                Query.OrderByDescending(a => a.Car.Year);
            else
                Query.OrderBy(a => a.Car.Year);
        }
    }
}