using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class CarDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string? Vin { get; set; }
    public string? Description { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string? Location { get; set; }
    public DrivetrainType Drivetrain { get; set; }
    public string? Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public CarStatus Status { get; set; }   
    public DateTime CreatedAt { get; set; }
    public int? AssingId { get; set; }
    public int OwnerId { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public ICollection<CarImageDto>? Images { get; set; }
}