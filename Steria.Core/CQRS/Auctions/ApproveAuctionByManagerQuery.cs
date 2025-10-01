using MediatR;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Auctions;

public class ApproveAuctionByManagerQuery : IRequest
{
    public int AuctionId { get; set; }
}

public class ApproveAuctionByManagerQueryHandler(IGenericRepository<Auction> auctionRepository) : IRequestHandler<ApproveAuctionByManagerQuery>
{
    public async Task Handle(ApproveAuctionByManagerQuery request, CancellationToken cancellationToken)
    {
        var auction = await auctionRepository.GetByIdAsync(request.AuctionId);
        
        auction.Status = AuctionStatus.Approved;

        await auctionRepository.UpdateAsync(auction);
    }
}