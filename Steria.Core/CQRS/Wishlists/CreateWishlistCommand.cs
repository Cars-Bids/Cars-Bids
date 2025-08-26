using AutoMapper;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

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