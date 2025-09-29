using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.WishlistSpec;

namespace Steria.Core.CQRS.Wishlists;
public class GetFilteredWishlistsQuery : IRequest<PagedResult<WishlistFilteredDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? EndingSoon { get; set; }
    public bool? NewCars { get; set; }
    public bool? Inspected { get; set; }
}

public class GetFilteredWishlistsQueryHandler(
    IGenericRepository<Wishlist> repository,
    IMapper mapper
) : IRequestHandler<GetFilteredWishlistsQuery, PagedResult<WishlistFilteredDto>>
{
    public async Task<PagedResult<WishlistFilteredDto>> Handle(GetFilteredWishlistsQuery request, CancellationToken cancellationToken)
    {
        var spec = new FilteredWishlistsSpec(request);
        var wishlists = await repository.GetListBySpec<Wishlist>(spec, cancellationToken);

        var dtos = mapper.Map<List<WishlistFilteredDto>>(wishlists);

        var countSpec = new FilteredWishlistsCountSpec(request);
        var totalCount = await repository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<WishlistFilteredDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}