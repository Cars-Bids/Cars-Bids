using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using MediatR;
using Steria.Core.Specification.ProfileSpec;

namespace Steria.Core.CQRS.Profile;

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