namespace Steria.Core.Entities;

public class Bid
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;

    public Auction Auction { get; set; } = null!;
    public User User { get; set; } = null!;
}
