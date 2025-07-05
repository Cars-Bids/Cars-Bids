using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Enums;

namespace CarsAndBids.Core.Interfaces;

public interface IAuctionService
{
    Task<IEnumerable<AuctionDto>> GetAllAsync();
    Task<IEnumerable<AuctionDto>> GetAllActiveAuctions();
    Task<AuctionDto?> GetById(int auctionId);
    Task<(bool Result, string? Error)> TryPlaceBid(int auctionId, decimal amount, string bidder);
    void UpdateStatus(int auctionId, AuctionStatus newStatus);
}
