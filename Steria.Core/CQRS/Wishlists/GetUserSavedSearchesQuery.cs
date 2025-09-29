using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.WishlistSpec;

namespace Steria.Core.CQRS.Wishlists;

public class GetUserSavedSearchesQuery : IRequest<PagedResult<SavedSearchDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserSavedSearchesQueryHandler(
    IGenericRepository<SavedSearch> savedSearchRepository,
    IGenericRepository<Auction> auctionRepository,
    IMapper mapper
) : IRequestHandler<GetUserSavedSearchesQuery, PagedResult<SavedSearchDto>>
{
    public async Task<PagedResult<SavedSearchDto>> Handle(GetUserSavedSearchesQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserSavedSearchesSpec(request.UserId, request.PageNumber, request.PageSize);
        var searches = await savedSearchRepository.GetListBySpec<SavedSearch>(spec, cancellationToken);

        var dtos = mapper.Map<List<SavedSearchDto>>(searches);

        foreach (var dto in dtos)
        {
            var auctionSpec = new FirstAuctionByMakeAndModelSpec(dto.MakeId, dto.ModelId);
            var firstAuction = await auctionRepository.GetItemBySpec<Auction>(auctionSpec, cancellationToken);
            if (firstAuction != null)
            {
                dto.FirstAuction = mapper.Map<AuctionFilteredDto>(firstAuction);
            }

            var auctionCountSpec = new AuctionsByMakeAndModelCountSpec(dto.MakeId, dto.ModelId);
            var totalMatchingAuctions = await auctionRepository.CountAsync(auctionCountSpec, cancellationToken);
            dto.TotalMatchingAuctions = totalMatchingAuctions;
        }

        var countSpec = new UserSavedSearchesCountSpec(request.UserId);
        var totalCount = await savedSearchRepository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<SavedSearchDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}