using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ProfileSpec;

namespace Steria.Core.CQRS.Profile;

public class GetUserActiveAuctionsQuery : IRequest<PagedResult<AuctionWithCarDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserActiveAuctionsQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetUserActiveAuctionsHandler(
    IGenericRepository<Auction> auctionRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserActiveAuctionsQuery, PagedResult<AuctionWithCarDto>>
{
    public async Task<PagedResult<AuctionWithCarDto>> Handle(GetUserActiveAuctionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserActiveAuctionsSpec(request.UserId, request.PageNumber, request.PageSize);
        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);

        var auctionWithCarDtos = mapper.Map<List<AuctionWithCarDto>>(auctions);

        var countSpec = new UserActiveAuctionsCountSpec(request.UserId);
        var totalCount = await auctionRepository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<AuctionWithCarDto>
        {
            Items = auctionWithCarDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}