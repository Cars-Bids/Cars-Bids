using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Wishlists;

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
