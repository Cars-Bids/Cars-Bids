using MediatR;
using System.Net;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Auctions;

public class DeleteAuctionByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteAuctionByIdCommandHandler(
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<DeleteAuctionByIdCommand>
{
    public async Task Handle(DeleteAuctionByIdCommand cmd, CancellationToken cancellationToken)
    {
        var auction = await auctionRepository.GetByIdAsync(cmd.Id)
            ?? throw new HttpException(string.Format(Resource.AuctionNotFoundById, cmd.Id), HttpStatusCode.NotFound);

        await auctionRepository.DeleteAsync(auction.Id);
    }
}
