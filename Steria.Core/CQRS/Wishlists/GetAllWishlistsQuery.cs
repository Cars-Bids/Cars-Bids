using Ardalis.Specification;
using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.WishlistSpec;


namespace Steria.Core.CQRS.Wishlists;

public class GetAllWishlistsQuery : IRequest<PagedResult<WishlistDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllWishlistsHandler(
    IMapper mapper,
    IGenericRepository<Wishlist> repository
) : IRequestHandler<GetAllWishlistsQuery, PagedResult<WishlistDto>>
{
    public async Task<PagedResult<WishlistDto>> Handle(GetAllWishlistsQuery request, CancellationToken cancellationToken)
    {
        var spec = new PagedWishlistsSpec(request.PageNumber, request.PageSize);

        var totalCount = await repository.CountAsync(new Specification<Wishlist>(), cancellationToken);
        var wishlists = await repository.GetListBySpec(spec, cancellationToken);

        var dtoList = mapper.Map<List<WishlistDto>>(wishlists);

        return new PagedResult<WishlistDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
