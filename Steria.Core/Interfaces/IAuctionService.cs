using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Enums;

namespace CarsAndBids.Core.Interfaces;

public interface IAuctionService
{
    Task<IEnumerable<AuctionDto>> GetAllOpenedAuctions();
    Task<AuctionDto?> GetById(int auctionId);
    Task<(bool Result, string? Error)> TryPlaceBid(int auctionId, decimal amount, string bidderName, int bidderUserId);
    void UpdateStatus(int auctionId, AuctionStatus newStatus);
}
