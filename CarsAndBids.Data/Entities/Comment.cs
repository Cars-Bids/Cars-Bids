using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarsAndBids.Data.Entities;

public class Comment
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Auction? Auction { get; set; }
    public User? User { get; set; }
}
