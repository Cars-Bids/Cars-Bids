using Steria.Core.Enums;

namespace Steria.Core.Entities;

public class Car
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Vin { get; set; } = null!;
    public string? Highlights { get; set; }
    public string? ServiceHistory { get; set; }
    public string? Equipment { get; set; }
    public string? Flaws { get; set; }
    public string? Modifications { get; set; }
    public string? OtherItems { get; set; }
    public string? OwnershipHistory { get; set; }
    public string? SellerNotes { get; set; }
    public string? About { get; set; }
    public string? VideoLinks { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string? Location { get; set; }
    public bool IsOnSaleElsewhere { get; set; }
    public bool IsModified { get; set; }
    public DrivetrainType? Drivetrain { get; set; }
    public string? Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int? Speeds { get; set; }
    public CarStatus Status { get; set; } = CarStatus.inPending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? ManagerId { get; set; }
    public int OwnerId { get; set; }
    public int? BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public int? ChatId { get; set; }

    public User? Manager { get; set; }
    public User Owner { get; set; } = null!;
    public BodyStyle? BodyStyle { get; set; } = null!;
    public Model Model { get; set; } = null!;
    public Chat? Chat { get; set; } = null!;
    public Auction? Auction { get; set; }
    public ICollection<CarImage>? Images { get; set; }
}