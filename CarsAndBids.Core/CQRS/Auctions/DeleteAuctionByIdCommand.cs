using MediatR;
using System.Net;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Interfaces;

namespace CarsAndBids.Core.CQRS.Auctions;

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
            ?? throw new HttpException($"Auction with id [{cmd.Id}] not found!", HttpStatusCode.NotFound);

        await auctionRepository.DeleteAsync(auction.Id);
    }
}
