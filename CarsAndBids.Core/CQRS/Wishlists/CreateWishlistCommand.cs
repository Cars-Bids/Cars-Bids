using AutoMapper;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Wishlists;

public class CreateWishlistCommand : IRequest
{
    public int UserId { get; set; }
    public int AuctionId { get; set; }
}

public class CreateWishlistCommandHandler(
    IGenericRepository<Wishlist> repository,
    IMapper mapper
    ) : IRequestHandler<CreateWishlistCommand>
{
    public async Task Handle(CreateWishlistCommand cmd, CancellationToken cancellationToken)
    {
        var wishlist = mapper.Map<Wishlist>(cmd);
        await repository.InsertAsync(wishlist);
    }
}