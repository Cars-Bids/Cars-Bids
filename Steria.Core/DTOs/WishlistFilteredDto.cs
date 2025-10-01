namespace Steria.Core.DTOs;
public class WishlistFilteredDto
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public AuctionDto Auction { get; set; }
}