using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsAndBids.Data.Entities;

public class Bid
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public required User Bidder { get; set; }

    [Required]
    public int AuctionId { get; set; }

    [ForeignKey("AuctionId")]
    public required Auction Auction { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime BidTime { get; set; } = DateTime.UtcNow;
}
