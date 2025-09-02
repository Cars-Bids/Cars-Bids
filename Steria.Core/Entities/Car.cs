using Steria.Core.Enums;

namespace Steria.Core.Entities;

public class Car
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Vin { get; set; } = null!;
    public string? Highlights { get; set; } = null!;
    public string? ServiceHistory { get; set; } = null!;
    public string? Equipment { get; set; } = null!;
    public string? Flaws { get; set; } = null!;
    public string? Modifications { get; set; } = null!;
    public string? OtherItems { get; set; } = null!;
    public string? OwnershipHistory { get; set; } = null!;
    public string? SellerNotes { get; set; } = null!;
    public string? VideoLinks { get; set; } = null!;
    public string? ExteriorColor { get; set; } = null!;
    public string? InteriorColor { get; set; } = null!;
    public int Mileage { get; set; }
    public string? Location { get; set; } = null!;
    public bool IsOnSaleElsewhere { get; set; }
    public bool IsModified { get; set; }
    public DrivetrainType? Drivetrain { get; set; }
    public string? Engine { get; set; } = null!;
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