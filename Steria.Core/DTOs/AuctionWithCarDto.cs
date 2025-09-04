namespace Steria.Core.DTOs;

public class AuctionWithCarDto
{
    public int Id { get; set; }
    public ProfileEndedCarDto Car { get; set; } = null!;
    public int SellerId { get; set; }
    public decimal StartPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public string? CurrentBidder { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string Status { get; set; } = null!; // Changed from AuctionStatus to string
}