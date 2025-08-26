using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.ProfileSpec;
using MediatR;

namespace CarsAndBids.Core.CQRS.Profile;

public class GetUserEndedAuctionsQuery : IRequest<PagedResult<AuctionDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetUserEndedAuctionsQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetUserEndedAuctionsHandler(
    IGenericRepository<Auction> auctionRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserEndedAuctionsQuery, PagedResult<AuctionDto>>
{
    public async Task<PagedResult<AuctionDto>> Handle(GetUserEndedAuctionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserEndedAuctionsSpec(request.UserId, request.PageNumber, request.PageSize);
        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);

        var auctionDtos = mapper.Map<List<AuctionDto>>(auctions);

        var totalCount = await auctionRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<AuctionDto>
        {
            Items = auctionDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}