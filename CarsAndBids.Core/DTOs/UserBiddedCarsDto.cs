namespace CarsAndBids.Core.DTOs;

public class UserBiddedCarsDto
{
    public int CarId { get; set; }
    public string MainImage { get; set; }
    public int BidCount { get; set; }
    public decimal LastBidAmount { get; set; }
    public string CarName { get; set; }
    public string Engine { get; set; }
    public string Drivetrain { get; set; }
    public string Transmission { get; set; }
    public string BodyStyle { get; set; }
    public string ExteriorColor { get; set; }
    public string InteriorColor { get; set; }
    public DateTime BidTime { get; set; }
}