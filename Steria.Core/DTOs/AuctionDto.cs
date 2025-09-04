using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class AuctionDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int SellerId { get; set; }
    public bool IsInspected { get; set; }
    public decimal StartPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public string? CurrentBidder { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public AuctionStatus Status { get; set; }
    public CarPreviewDto Car { get; set; }
}
