using Steria.Core.Enums;

namespace Steria.Core.DTOs;

public class CarManagerDto
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string? Vin { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string? Location { get; set; }
    public bool IsOnSaleElsewhere { get; set; }
    public bool IsModified { get; set; }
    public DrivetrainType Drivetrain { get; set; }
    public string? Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public CarStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ManagerId { get; set; }
    public string Owner { get; set; }
    public string BodyStyle { get; set; }
    public string Model { get; set; }
    public string Make { get; set; }
    public ICollection<CarImageDto>? Images { get; set; }
}