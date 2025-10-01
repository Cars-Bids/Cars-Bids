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
        var spec = new FilteredAuctionsSpec(request);
        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);

        var auctionDtos = mapper.Map<List<AuctionFilteredDto>>(auctions);

        var modelIds = auctions.Select(a => a.Car.ModelId).Distinct().ToList();
        var makeIds = auctions.Select(a => a.Car.Model.MakeId).Distinct().ToList();

        var countSpec = new FilteredAuctionsCountSpec(request);
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