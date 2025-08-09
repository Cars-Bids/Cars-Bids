using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Wishlists;

public class GetWishlistByIdQuery : IRequest<WishlistDto?>
{
    public int Id { get; set; }
}

public class GetWishlistByIdHandler(
    IMapper mapper,
    IGenericRepository<Wishlist> repository
    ) : IRequestHandler<GetWishlistByIdQuery, WishlistDto?>
{
    public async Task<WishlistDto?> Handle(GetWishlistByIdQuery request, CancellationToken cancellationToken)
    {
        var wishlist = await repository.GetByIdAsync(request.Id);

        return wishlist is null
            ? null
            : mapper.Map<WishlistDto>(wishlist);
    }
}
