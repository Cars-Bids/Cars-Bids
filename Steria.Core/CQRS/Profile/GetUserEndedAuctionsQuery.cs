using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.ProfileSpec;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Steria.Core.CQRS.Profile;

public class GetUserEndedAuctionsQuery : IRequest<PagedResult<AuctionWithCarDto>>
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
    IMapper mapper,
    ILogger<GetUserEndedAuctionsHandler> logger
    ) : IRequestHandler<GetUserEndedAuctionsQuery, PagedResult<AuctionWithCarDto>>
{
    public async Task<PagedResult<AuctionWithCarDto>> Handle(GetUserEndedAuctionsQuery request, CancellationToken cancellationToken)
    {
        var spec = new UserEndedAuctionsSpec(request.UserId, request.PageNumber, request.PageSize);
        var auctions = await auctionRepository.GetListBySpec(spec, cancellationToken);

        // Логування для дебагу (опціонально)
        foreach (var auction in auctions)
        {
            logger.LogInformation("Auction ID: {Id}, Car ID: {CarId}, Car Name: {CarName}, Main Image: {MainImage}, Status: {Status}",
                auction.Id,
                auction.Car?.Id,
                auction.Car != null ? $"{auction.Car.Year} {auction.Car.Model?.Make?.Name} {auction.Car.Model?.Name}" : "Unknown",
                auction.Car?.Images?.FirstOrDefault()?.ImageUrl ?? "None",
                auction.Status.ToString());
        }

        var auctionWithCarDtos = mapper.Map<List<AuctionWithCarDto>>(auctions);

        // Використовуємо специфікацію для підрахунку без пагінації
        var countSpec = new UserEndedAuctionsCountSpec(request.UserId);
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