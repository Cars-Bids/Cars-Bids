using CarsAndBids.Core.Enums;

namespace CarsAndBids.Core.Entities;

public class CarImage
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public ImageCategory ImageCategory { get; set; }
    public int OrderNumber { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Car Car { get; set; } = null!;
}