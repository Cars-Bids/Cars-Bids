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
        // Отримуємо останні унікальні ставки (по одній на кожен car) з БД
        var bids = await bidRepository.GetLatestUniqueBidsByUserAsync(
            request.UserId,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        // Якщо потрібно totalCount для пагінації — використаємо існуючий метод
        var totalCount = await bidRepository.CountUniqueCarsAsync(request.UserId, cancellationToken);

        // Отримуємо counts для carId (щоб заповнити BidCount)
        var carIds = bids
            .Where(b => b.Auction != null)
            .Select(b => b.Auction.CarId)
            .Distinct()
            .ToList();

        var countsDict = await bidRepository.GetBidCountsForCarIdsAsync(request.UserId, carIds, cancellationToken);

        // Мапимо в DTO — без групування в пам'яті
        var items = bids.Select(b =>
        {
            var dto = mapper.Map<UserBiddedCarsDto>(b);
            var carId = b.Auction?.CarId ?? 0;
            dto.BidCount = countsDict.TryGetValue(carId, out var c) ? c : 0;
            dto.LastBidAmount = b.BidAmount;
            dto.BidTime = b.BidTime;
            // MainImage — mapper може встановлювати, але якщо ні, можна взяти тут:
            dto.MainImage = b.Auction?.Car?.Images?
                .FirstOrDefault(img => img.ImageCategory == ImageCategory.Main)?.ImageUrl
                ?? b.Auction?.Car?.Images?.FirstOrDefault()?.ImageUrl ?? string.Empty;
            return dto;
        }).ToList();

        return new PagedResult<UserBiddedCarsDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

}