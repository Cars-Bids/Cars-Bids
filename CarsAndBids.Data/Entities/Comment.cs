using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsAndBids.Data.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public required User Author { get; set; }

    [Required]
    public int AuctionId { get; set; }

    [ForeignKey("AuctionId")]
    public required Auction Auction { get; set; }

    [Required]
    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
