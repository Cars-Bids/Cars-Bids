using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsAndBids.Data.Entities;

public class Bid
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;

    public Auction? Auction { get; set; }
    public User? User { get; set; }
}
