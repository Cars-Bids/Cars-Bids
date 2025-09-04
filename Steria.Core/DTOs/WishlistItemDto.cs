namespace Steria.Core.DTOs;

public class WishlistItemDto
{
    public int AuctionId { get; set; }
    public decimal StartPrice { get; set; }
    public decimal? CurrentPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public int CarId { get; set; }
    public string CarName { get; set; }
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string BodyStyle { get; set; }
    public string ExteriorColor { get; set; }
    public string InteriorColor { get; set; }
    public string Engine { get; set; }
    public string Drivetrain { get; set; }
    public string TransmissionType { get; set; }
    public string MainImage { get; set; }
    public DateTime AddedAt { get; set; }
}