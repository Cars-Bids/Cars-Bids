namespace Steria.Core.DTOs;

public class WishlistDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AuctionId { get; set; }
    public DateTime AddedAt { get; set; }
}

