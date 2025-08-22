using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.Profile;
using MediatR;

namespace CarsAndBids.Core.CQRS.Profile;

public class GetUserBidsAndWinsQuery : IRequest<UserBidsAndWinsDto>
{
    public int UserId { get; set; }
}

public class GetUserBidsAndWinsHandler(
    IGenericRepository<Bid> bidRepository,
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<GetUserBidsAndWinsQuery, UserBidsAndWinsDto>
{
    public async Task<UserBidsAndWinsDto> Handle(GetUserBidsAndWinsQuery request, CancellationToken cancellationToken)
    {
        var bidsCount = await bidRepository.CountAsync(new UserBidsCountSpec(request.UserId), cancellationToken);
        var winsCount = await auctionRepository.CountAsync(new UserWinsCountSpec(request.UserId), cancellationToken);

        return new UserBidsAndWinsDto
        {
            TotalBids = bidsCount,
            TotalWins = winsCount
        };
    }
}