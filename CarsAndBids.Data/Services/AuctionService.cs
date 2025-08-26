using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Enums;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.ProfileSpec;

namespace CarsAndBids.Data.Services;

public class AuctionService(
    IMapper mapper,
    IGenericRepository<Bid> bidRepository,
    IGenericRepository<Auction> auctionRepository
    ) : IAuctionService
{   
    public async Task<IEnumerable<AuctionDto>> GetAllOpenedAuctions()
    {
        var activeAuctions = await auctionRepository.GetAsync(
            filter: a => 
                a.Status == AuctionStatus.Active || 
                a.Status == AuctionStatus.Pending
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

    public async Task<(bool Result, string? Error)> TryPlaceBid(int auctionId, decimal amount, string bidderName, int bidderUserId)
    {
        await using var transaction = await auctionRepository.BeginTransactionAsync();

        try
        {
            var auction = await auctionRepository.GetByIdAsync(auctionId)
                ?? throw new Exception("Auction not found");

            if (auction.Status != AuctionStatus.Active || DateTime.UtcNow > auction.EndTime)
            {
                throw new Exception("Auction is not active or expired!");
            }

            if (amount <= auction.CurrentPrice)
            {
                throw new Exception("Bid must be higher than the current price!");
            }

            //Антиснайпер: подовжуємо, якщо менше 1 хв
            var remaining = auction.EndTime - DateTime.UtcNow;
            if (remaining.TotalMinutes < 1)
            {
                auction.EndTime = DateTime.UtcNow.AddMinutes(1);
            }
            auction.CurrentPrice = amount;
            auction.CurrentBidder = bidderName;
            await auctionRepository.UpdateAsync(auction);

            //add bid
            await bidRepository.InsertAsync(new Bid
            {
                AuctionId = auctionId,
                UserId = bidderUserId,
                BidAmount = amount,
                BidTime = DateTime.UtcNow
            });

            await transaction.CommitAsync();
            return (true, null);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return (false, e.Message);
        }        
    }
    public async Task<List<Auction>> GetUserAuctions(int userId)
    {
        var spec = new UserAuctionsSpec(userId);
        return await auctionRepository.GetListBySpec<Auction>(spec);
    }

    public async void UpdateStatus(int auctionId, AuctionStatus newStatus)
    {
        var auction = await auctionRepository.GetByIdAsync(auctionId);
        if (auction is null) return;

        auction.Status = newStatus;
        await auctionRepository.UpdateAsync(auction);
    }
}
