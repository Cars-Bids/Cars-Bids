using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Wishlists;

public class DeleteWishlistByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteWishlistByIdHandler(
    IGenericRepository<Wishlist> repository
    ) : IRequestHandler<DeleteWishlistByIdCommand>
{
    public async Task Handle(DeleteWishlistByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}
