using MediatR;
using System.Net;
using AutoMapper;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Auctions;

public class UpdateAuctionCommand : IRequest
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int SellerId { get; set; }
    public decimal StartPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public AuctionStatus Status { get; set; }
}

public class UpdateAuctionCommandHandler(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
) : IRequestHandler<UpdateAuctionCommand>
{
    public async Task Handle(UpdateAuctionCommand cmd, CancellationToken cancellationToken)
    {
        var existingAuction = await auctionRepository.GetByIdAsync(cmd.Id)
            ?? throw new HttpException(string.Format(Resource.AuctionNotFoundById, cmd.Id), HttpStatusCode.NotFound);

        mapper.Map(cmd, existingAuction);

        await auctionRepository.UpdateAsync(existingAuction);
    }
}
