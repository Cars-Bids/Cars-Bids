using AutoMapper;
using Steria.Core.DTOs;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

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
    }
}
