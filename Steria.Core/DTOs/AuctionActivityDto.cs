namespace Steria.Core.DTOs;

public class AuctionActivityDto
{
    public string Type { get; set; } // "Comment" or "Bid"
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? Text { get; set; }
    public int? Upvotes { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }

    public decimal? Amount { get; set; }
    public int? BidderId { get; set; }
    public string? BidderName { get; set; }
}