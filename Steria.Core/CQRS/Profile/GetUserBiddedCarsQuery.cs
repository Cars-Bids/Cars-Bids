using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using MediatR;
using Steria.Core.Specification.ProfileSpec;

namespace Steria.Core.CQRS.Profile;

public class GetUserBiddedCarsQuery : IRequest<PagedResult<UserBiddedCarsDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class GetUserBiddedCarsHandler(
    IGenericRepository<Bid> bidRepository,
    IMapper mapper
    ) : IRequestHandler<GetUserBiddedCarsQuery, PagedResult<UserBiddedCarsDto>>
{

    public async Task<PagedResult<UserBiddedCarsDto>> Handle(GetUserBiddedCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserBiddedCarsSpec(request.UserId, request.PageNumber, request.PageSize);
        var bids = await bidRepository.GetListBySpec(spec, cancellationToken);

        var groupedBids = bids
            .GroupBy(bid => bid.Auction.CarId)
            .Select(g =>
            {
                var firstBid = g.OrderByDescending(b => b.BidTime).First();
                var dto = mapper.Map<UserBiddedCarsDto>(firstBid);
                dto.BidCount = g.Count();
                dto.LastBidAmount = firstBid.BidAmount;
                dto.BidTime = firstBid.BidTime;
                dto.MainImage = g.First().Auction.Car.Images
                    .Where(img => img.ImageCategory == ImageCategory.Main)
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault() ?? g.First().Auction.Car.Images
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault() ?? "";
                return dto;
            })
            .ToList();

        var totalCount = await bidRepository.CountUniqueCarsAsync(request.UserId, cancellationToken);

        return new PagedResult<UserBiddedCarsDto>
        {
            Items = groupedBids,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}