using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class CarNewestDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string? Vin { get; set; }
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
    public int? ManagerId { get; set; }
    public int OwnerId { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public string? MakeName { get; set; }
    public string? ModelName { get; set; }
    public string? MainImage { get; set; }
    public string? ExteriorImage1 { get; set; }
    public string? ExteriorImage2 { get; set; }
}