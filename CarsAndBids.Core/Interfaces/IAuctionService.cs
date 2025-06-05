using CarsAndBids.Core.DTOs;

namespace CarsAndBids.Core.Interfaces;

public interface IAuctionService
{
    Task<IEnumerable<AuctionDto>> GetAllAsync();
}
