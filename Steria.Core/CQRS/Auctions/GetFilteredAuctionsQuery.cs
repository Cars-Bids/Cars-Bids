using Ardalis.Specification;
using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetFilteredAuctionsQuery : IRequest<FilteredAuctionsPagedResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? MakeModelSearch { get; set; }
    public string? Transmission { get; set; }
    public string? BodyStyle { get; set; }
    public int? MinMileage { get; set; }
    public int? MaxMileage { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
}

public class GetFilteredAuctionsQueryHandler(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<GetFilteredAuctionsQuery, FilteredAuctionsPagedResult>
{
    public async Task<FilteredAuctionsPagedResult> Handle(GetFilteredAuctionsQuery request, CancellationToken cancellationToken)
    {
        // Створюємо специфікацію для фільтрації та пагінації
        var spec = new FilteredAuctionsSpec(request);
        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);

        // Мапимо аукціони на DTO
        var auctionDtos = mapper.Map<List<AuctionFilteredDto>>(auctions);

        // Отримуємо унікальні ModelId і MakeId
        var modelIds = auctions.Select(a => a.Car.ModelId).Distinct().ToList();
        var makeIds = auctions.Select(a => a.Car.Model.MakeId).Distinct().ToList();

        // Отримуємо загальну кількість для пагінації
        var countSpec = new Specification<Auction>();
        countSpec.Query.Where(a => a.Status == AuctionStatus.Active);

        if (!string.IsNullOrWhiteSpace(request.Transmission))
        {
            if (Enum.TryParse<TransmissionType>(request.Transmission, true, out var transmissionType))
            {
                countSpec.Query.Where(a => a.Car.TransmissionType == transmissionType);
            }
        }
        if (!string.IsNullOrWhiteSpace(request.BodyStyle))
        {
            countSpec.Query.Where(a => a.Car.BodyStyle.StyleName == request.BodyStyle);
        }
        if (request.MinMileage.HasValue)
        {
            countSpec.Query.Where(a => a.Car.Mileage >= request.MinMileage.Value);
        }
        if (request.MaxMileage.HasValue)
        {
            countSpec.Query.Where(a => a.Car.Mileage <= request.MaxMileage.Value);
        }
        if (!string.IsNullOrWhiteSpace(request.MakeModelSearch))
        {
            var searchTerms = request.MakeModelSearch.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var term in searchTerms)
            {
                countSpec.Query.Where(a => a.Car.Model.Name.ToLower().Contains(term) ||
                                        a.Car.Model.Make.Name.ToLower().Contains(term));
            }
        }

        var totalCount = await auctionRepository.CountAsync(countSpec, cancellationToken);

        return new FilteredAuctionsPagedResult
        {
            Items = auctionDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            ModelIds = modelIds,
            MakeIds = makeIds
        };
    }
}