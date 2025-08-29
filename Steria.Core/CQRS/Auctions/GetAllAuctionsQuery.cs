using MediatR;
using AutoMapper;
using Ardalis.Specification;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetAllAuctionsQuery : IRequest<PagedResult<AuctionDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = int.MaxValue;
}

public class GetAllAuctionsQueryHandler(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<GetAllAuctionsQuery, PagedResult<AuctionDto>>
{
    public async Task<PagedResult<AuctionDto>> Handle(GetAllAuctionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new AuctionsWithCar(request.PageNumber, request.PageSize);
        var totalCount = await auctionRepository.CountAsync(new Specification<Auction>(), cancellationToken);

        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);

        var dtoList = mapper.Map<List<AuctionDto>>(auctions);

        return new PagedResult<AuctionDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
