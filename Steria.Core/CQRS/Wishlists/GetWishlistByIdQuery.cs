using System.Net;
using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
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
        var wishlist = await repository.GetByIdAsync(request.Id)
            ?? throw new HttpException("user not found", HttpStatusCode.NotFound);

        return mapper.Map<WishlistDto>(wishlist);
    }
}
