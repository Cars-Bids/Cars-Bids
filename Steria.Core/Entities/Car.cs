using Steria.Core.Enums;

namespace Steria.Core.Entities;

public class Car
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Vin { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ExteriorColor { get; set; } = null!;
    public string InteriorColor { get; set; } = null!;
    public int Mileage { get; set; }
    public string Location { get; set; } = null!;
    public DrivetrainType Drivetrain { get; set; }
    public string Engine { get; set; } = null!;
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public CarStatus Status { get; set; } = CarStatus.inPending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsInspected { get; set; }
    public bool OnReserve { get; set; }
    public int? AssingId { get; set; }
    public int OwnerId { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public int ChatId { get; set; }

    public User? Assing { get; set; }
    public User Owner { get; set; } = null!;
    public BodyStyle BodyStyle { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public Chat Chat { get; set; } = null!;
    public Auction? Auction { get; set; }
    public ICollection<CarImage>? Images { get; set; }
}