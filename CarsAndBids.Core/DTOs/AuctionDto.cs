using CarsAndBids.Core.Enums;

namespace CarsAndBids.Core.DTOs;

public class AuctionDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int SellerId { get; set; }
    public decimal StartPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public string? CurrentBidder { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public AuctionStatus Status { get; set; }
}
