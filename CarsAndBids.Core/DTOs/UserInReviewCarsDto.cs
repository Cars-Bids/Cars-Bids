namespace CarsAndBids.Core.DTOs;

public class UserInReviewCarsDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string BodyStyle { get; set; }
    public string ExteriorColor { get; set; }
    public string InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string Location { get; set; }
    public string MainImage { get; set; }
    public DateTime CreatedAt { get; set; }
}