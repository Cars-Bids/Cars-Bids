using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarsAndBids.Data.Enums;

namespace CarsAndBids.Data.Entities;

public class Auction
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int SellerId { get; set; }
    public decimal StartPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuctionStatus Status { get; set; } = AuctionStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }

    public Car Car { get; set; } = null!;
    public User Seller { get; set; } = null!;
    public ICollection<Bid>? Bids { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Question>? Questions { get; set; }
    public ICollection<Wishlist>? Wishlists { get; set; }
}
