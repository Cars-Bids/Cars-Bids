using Steria.Core.DTOs;

namespace Steria.Core.Interfaces;

public interface IAuctionRepository
{
    Task<AuctionData?> GetAuctionByIdAsync(int auctionId, int userId);
    Task<CarData?> GetAuctionCarByIdAsync(int carId);
    Task<List<CarImageData>> GetCarImagesByCarIdAsync(int carId);
    Task<List<QAData>> GetQaByAuctionIdAsync(int auctionId);
    Task<List<CommentData>> GetCommentsByAuctionIdAsync(int auctionId);
    Task<List<OtherAuction>> GetOtherAuctionsAsync(int auctionId, int userId);
}
