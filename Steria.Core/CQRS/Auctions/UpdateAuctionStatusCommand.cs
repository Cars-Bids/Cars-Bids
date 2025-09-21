using MediatR;
using System.Net;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Auctions;

public class UpdateAuctionStatusCommand : IRequest
{
    public int Id { get; set; }
    public AuctionStatus Status { get; set; }
}

public class UpdateAuctionStatusCommandHandler(
    IGenericRepository<Auction> auctionRepository
) : IRequestHandler<UpdateAuctionStatusCommand>
{
    public async Task Handle(UpdateAuctionStatusCommand cmd, CancellationToken cancellationToken)
    {
        var existingAuction = await auctionRepository.GetByIdAsync(cmd.Id)
            ?? throw new HttpException(string.Format(Resource.AuctionNotFoundById, cmd.Id), HttpStatusCode.NotFound);

        existingAuction.Status = cmd.Status;
        await auctionRepository.UpdateAsync(existingAuction);
    }
}
