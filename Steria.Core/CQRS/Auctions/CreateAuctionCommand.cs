using MediatR;
using AutoMapper;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Auctions;

public class CreateAuctionCommand : IRequest
{
    public int CarId { get; set; }
    public int SellerId { get; set; }
    public decimal StartPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class CreateAuctionCommandHandler(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<CreateAuctionCommand>
{    
    public async Task Handle(CreateAuctionCommand cmd, CancellationToken cancellationToken)
    {

        var auction = mapper.Map<Auction>(cmd);
        auction.CurrentPrice = 0;
        auction.Status = AuctionStatus.Pending;
        auction.CreatedAt = DateTime.UtcNow;

        await auctionRepository.InsertAsync(auction);
    }
}