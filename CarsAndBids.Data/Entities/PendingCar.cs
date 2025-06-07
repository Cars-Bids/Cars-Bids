using CarsAndBids.Data.Enums;

namespace CarsAndBids.Data.Entities;

public class PendingCar
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int Year { get; set; }
    public string Vin { get; set; }
    public string ExteriorColor { get; set; }
    public string InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string Location { get; set; }
    public DrivetrainType Drivetrain { get; set; }
    public string Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public string Modifications { get; set; }
    public string Flaws { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int ModelId { get; set; }
    public int BodyStyleId { get; set; }

    public User Owner { get; set; }
    public Model Model { get; set; }
    public BodyStyle BodyStyle { get; set; }
}