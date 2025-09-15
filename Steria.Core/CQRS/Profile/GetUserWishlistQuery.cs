using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ProfileSpec;

namespace Steria.Core.CQRS.Profile;

public class GetUserWishlistQuery : IRequest<PagedResult<WishlistItemDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserWishlistQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetUserWishlistHandler(
    IGenericRepository<Wishlist> wishlistRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserWishlistQuery, PagedResult<WishlistItemDto>>
{
    public async Task<PagedResult<WishlistItemDto>> Handle(GetUserWishlistQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserWishlistSpec(request.UserId, request.PageNumber, request.PageSize);
        var wishlistItems = await wishlistRepository.GetListBySpec(spec, cancellationToken);

        var totalCount = await wishlistRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<WishlistItemDto>
        {
            Items = wishlistItems,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}