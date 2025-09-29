using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

public class DeleteWishlistByAuctionCommand : IRequest
{
    public int UserId { get; set; }
    public int AuctionId { get; set; }
}

public class DeleteWishlistByAuctionCommandHandler(
    IGenericRepository<Wishlist> repository
    ) : IRequestHandler<DeleteWishlistByAuctionCommand>
{
    public async Task Handle(DeleteWishlistByAuctionCommand cmd, CancellationToken cancellationToken)
    {
        var res = await repository.GetAsync(filter: x =>
            x.UserId == cmd.UserId && 
            x.AuctionId == cmd.AuctionId
        );

        await repository.DeleteRangeAsync(res);
    }
}
