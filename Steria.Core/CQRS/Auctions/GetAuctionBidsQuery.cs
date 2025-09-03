using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class GetAuctionBidsQuery : IRequest<List<AuctionActivityDto>>
{
    public int AuctionId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetAuctionBidsQueryHandler(IGenericRepository<Bid> bidRepository,
                                        IMapper mapper) : IRequestHandler<GetAuctionBidsQuery, List<AuctionActivityDto>>
{
    public async Task<List<AuctionActivityDto>> Handle(GetAuctionBidsQuery request, CancellationToken cancellationToken)
    {
        var spec = new AuctionBidsSpec(request.AuctionId, request.PageNumber, request.PageSize);
        var bids = await bidRepository.GetListBySpec(spec, cancellationToken);
        return mapper.Map<List<AuctionActivityDto>>(bids);
    }
}