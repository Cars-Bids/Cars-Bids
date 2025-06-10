using CarsAndBids.Data.Enums;

namespace CarsAndBids.Core.DTOs;

public class CarImageDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public string? ImageUrl { get; set; }
    public ImageCategory ImageCategory { get; set; }
    public int OrderNumber { get; set; }
    public DateTime UploadedAt { get; set; }
}