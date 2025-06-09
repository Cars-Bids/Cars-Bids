namespace CarsAndBids.Data.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AuctionId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Auction? Auction { get; set; }
}