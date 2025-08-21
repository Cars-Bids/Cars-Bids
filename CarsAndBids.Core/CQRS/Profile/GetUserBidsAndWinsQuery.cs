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

public class GetUserBidsAndWinsHandler : IRequestHandler<GetUserBidsAndWinsQuery, UserBidsAndWinsDto>
{
    private readonly IGenericRepository<Bid> _bidRepository;
    private readonly IGenericRepository<Auction> _auctionRepository;

    public GetUserBidsAndWinsHandler(
        IGenericRepository<Bid> bidRepository,
        IGenericRepository<Auction> auctionRepository)
    {
        _bidRepository = bidRepository;
        _auctionRepository = auctionRepository;
    }

    public async Task<UserBidsAndWinsDto> Handle(GetUserBidsAndWinsQuery request, CancellationToken cancellationToken)
    {
        var bidsCount = await _bidRepository.CountAsync(new UserBidsCountSpec(request.UserId), cancellationToken);
        var winsCount = await _auctionRepository.CountAsync(new UserWinsCountSpec(request.UserId), cancellationToken);

        return new UserBidsAndWinsDto
        {
            TotalBids = bidsCount,
            TotalWins = winsCount
        };
    }
}