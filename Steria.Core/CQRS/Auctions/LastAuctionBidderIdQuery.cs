using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.AuctionSpec;

namespace Steria.Core.CQRS.Auctions;

public class LastAuctionBidderIdQuery : IRequest<int>
{
    public int AuctionId { get; set; }
}

public class LastAuctionBidderIdQueryHandler(IGenericRepository<Auction> auctionRepository) : IRequestHandler<LastAuctionBidderIdQuery, int>
{
    public async Task<int> Handle(LastAuctionBidderIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new LastBidderIdSpec(request.AuctionId);
        return await auctionRepository.GetItemBySpec(spec, cancellationToken);
    }
}