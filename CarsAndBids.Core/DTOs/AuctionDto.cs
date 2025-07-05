namespace CarsAndBids.Core.DTOs;

public class AuctionDto
{
    public int Id { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string? Description { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal CurrentBid { get; set; }
    public string? CurrentBidder { get; set; }
    public DateTime EndTime { get; set; }
    public int Mileage { get; set; }
    public string? Status { get; set; }
    public string? VIN { get; set; }
    public string? Location { get; set; }
    public int UserId { get; set; }
}
