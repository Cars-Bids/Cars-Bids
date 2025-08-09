using MediatR;
using System.Net;
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.CQRS.Auctions;

public class GetAuctionByIdQuery : IRequest<AuctionDto>
{
    public int Id { get; set; }
}

public class GetAuctionByIdQueryHandler(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
    ) : IRequestHandler<GetAuctionByIdQuery, AuctionDto>
{
    public async Task<AuctionDto> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await auctionRepository.GetByIdAsync(request.Id)
            ?? throw new HttpException(string.Format(Resource.AuctionNotFoundById, request.Id), HttpStatusCode.NotFound);

        var auctionDto = mapper.Map<AuctionDto>(auction);

        return auctionDto;
    }
}