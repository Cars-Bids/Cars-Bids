using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;
using CarsAndBids.Core.Interfaces;

namespace CarsAndBids.Data.Services;

public class AuctionService(
    IMapper mapper,
    IGenericRepository<Auction> auctionRepository
    ) : IAuctionService
{   
    public async Task<IEnumerable<AuctionDto>> GetAllAsync()
    {
        var auctions = await auctionRepository.GetAsync();
        return mapper.Map<IEnumerable<AuctionDto>>(auctions);
    }

    public async Task<IEnumerable<AuctionDto>> GetAllActiveAuctions()
    {
        var activeAuctions = await auctionRepository.GetAsync(
            filter: a => a.Status == AuctionStatus.Active
        );
        return mapper.Map<IEnumerable<AuctionDto>>(activeAuctions);
    }

    public async Task<AuctionDto?> GetById(int auctionId)
    {
        var auction = await auctionRepository.GetByIdAsync(auctionId);
        return auction is null 
            ? null 
            : mapper.Map<AuctionDto>(auction);
    }

    public async Task<(bool Result, string? Error)> TryPlaceBid(int auctionId, decimal amount, string bidder)
    {
        var auction = await auctionRepository.GetByIdAsync(auctionId);

        if (auction is null)
        {
            return (false, "Аукціон не знайдено!");
        }

        if (auction.Status != AuctionStatus.Active || DateTime.UtcNow > auction.EndTime)
        {
            return (false, "Аукціон не активний або завершено!");
        }

        if (amount <= auction.CurrentPrice)
        {
            return (false, "Ставка має бути вищою за поточну!");
        }

        //Антиснайпер: подовжуємо, якщо менше 1 хв
        var remaining = auction.EndTime - DateTime.UtcNow;
        if (remaining.TotalMinutes < 1)
        {
            auction.EndTime = DateTime.UtcNow.AddMinutes(1);
        }
        auction.CurrentPrice = amount;
        auction.CurrentBidder = bidder;

        //todo - save bid!
        await auctionRepository.UpdateAsync(auction);
        return (true, null);
    }

    public async void UpdateStatus(int auctionId, AuctionStatus newStatus)
    {
        var auction = await auctionRepository.GetByIdAsync(auctionId);
        if (auction is null) return;

        auction.Status = newStatus;
        await auctionRepository.UpdateAsync(auction);
    }
}
