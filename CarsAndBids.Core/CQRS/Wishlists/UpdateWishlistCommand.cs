using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Wishlists;

public class UpdateWishlistCommand : IRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AuctionId { get; set; }
}

public class UpdateWishlistCommandHandler(
    IGenericRepository<Wishlist> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateWishlistCommand>
{
    public async Task Handle(UpdateWishlistCommand cmd, CancellationToken cancellationToken)
    {
        var existingWishlist = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingWishlist);

        await repository.UpdateAsync(existingWishlist!);
        return;
    }
}
