using MediatR;
using System.Net;
using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Auctions;

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
            ?? throw new HttpException($"Auction with id [{request.Id}] not found!", HttpStatusCode.NotFound);

        var auctionDto = mapper.Map<AuctionDto>(auction);

        return auctionDto;
    }
}