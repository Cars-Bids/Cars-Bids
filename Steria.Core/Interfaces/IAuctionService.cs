using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Enums;

namespace Steria.Core.Interfaces;

public interface IAuctionService
{
    Task<IEnumerable<AuctionDto>> GetAllOpenedAuctions();
    Task<AuctionDto?> GetById(int auctionId);
    Task<(bool Result, string? Error)> TryPlaceBid(int auctionId, decimal amount, string bidderName, int bidderUserId);
    Task UpdateStatus(int auctionId, AuctionStatus newStatus);
    Task<List<Auction>> GetUserAuctions(int userId);
}
