using Steria.Core.Enums;

namespace Steria.Core.DTOs;
public class ProfileEndedCarDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string BodyStyle { get; set; } = null!; // Changed from enum to string
    public string Drivetrain { get; set; } = null!; // Changed from enum to string
    public string TransmissionType { get; set; } = null!; // Changed from enum to string
    public string ExteriorColor { get; set; } = null!;
    public string InteriorColor { get; set; } = null!;
    public string Engine { get; set; } = null!;
    public string Status { get; set; }
    public string MainImage { get; set; } = null!; // Single main image URL
}