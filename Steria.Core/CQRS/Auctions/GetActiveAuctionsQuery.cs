using MediatR;
using AutoMapper;
using Ardalis.Specification;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetActiveAuctionsQuery : IRequest<List<AuctionWithCarDto>>
{
    public int Count { get; set; } = 10;
}

public class GetActiveAuctionsQueryHandler(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<GetActiveAuctionsQuery, List<AuctionWithCarDto>>
{
    public async Task<List<AuctionWithCarDto>> Handle(GetActiveAuctionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ActiveAuctionsSpec(request.Count);
        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);
        return mapper.Map<List<AuctionWithCarDto>>(auctions);
    }
}